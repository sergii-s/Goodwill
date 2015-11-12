using System;
using System.Collections.Generic;
using System.Linq;
using Goodwill.Core.Events;

namespace Goodwill.Core
{
    public class Goodwill : IGoodwill
    {
        public IGameParameters Config { get; }
        private readonly IGameInitializer _gameInitializer;
        private int _currentYear = 1;

        public Goodwill()
            : this(new DefaultGameParameters(), new GameInitializer())
        {
        }

        public Goodwill(IGameParameters config, IGameInitializer gameInitializer)
        {
            Config = config;
            _gameInitializer = gameInitializer;
        }

        public List<Company> Companies { get; } = new List<Company>();

        public List<Player> Players { get; } = new List<Player>();

        public Deck<Manager> Managers { get; } = new Deck<Manager>();

        public Deck<IGameEventAction> Events { get; set; }
        public Deck<int> Probabilities { get; set; }

        public IDictionary<Ressource, int> RessourcePrices { get; } = new Dictionary<Ressource, int>();

        public GameState GameState { get; set; }

        private IDictionary<string, int> _playerBids { get; set; }

        public Player AddPlayer(string playerName)
        {
            var newPlayer = new Player
            {
                Name = playerName
            };
            Players.Add(newPlayer);

            return newPlayer;
        }

        public void Start()
        {
            if (Players.Count < 2)
            {
                throw new Exception("Should be at least 2 players");
            }
            _gameInitializer.InitializeGame(this, Config);
            BeginYear();
        }

        public GameInfo GetGameInfo()
        {
            return new GameInfo
            {
                CurrentYear = _currentYear,
                TotalYears = Config.Years,
                Companies = Companies.Select(x => new CompanyInfo
                {
                    Name = x.Name,
                    Money = x.Money,
                    MarketShare = x.MarketShare,
                    Manager = x.Manager,
                    RessourceDependencies = x.RessourceDependencies.Select((x1, i) =>
                        new RessourceInfo
                        {
                            Index = i,
                            Ressource = x1,
                            RessourceName = x1.ToString()
                        }
                    ).ToList()
                }).ToDictionary(x => x.Name, x => x),
                Ressources = RessourcePrices.ToDictionary(x => x.Key, x => x.Value),
                Players = Players.Select(x => new PlayerInfo
                {
                    Name = x.Name,
                    Money = x.Money,
                    Actions = x.Actions.Select(s => new ActionInfo
                    {
                        Company = s.Company.Name
                    }).ToList(),
                    Events = x.Events
                }).ToList(),
                State = GameState
            };
        }

        public void SetPrice(string player, string company, int price)
        {
            if (!Equals(GameState, GameState.Pricing.Company(company)))
            {
                throw new Exception();
            }
            _playerBids[player] = price;
        }

        public void VoteManager(string player, string company, string manager)
        {
            if (!Equals(GameState, GameState.VotingForManager.Company(company)))
            {
                throw new Exception();
            }
        }

        private void BeginYear()
        {
            TakeEvents();
            GameState = GameState.Pricing.Company(Config.CompanyEvaluatingOrderByYear[_currentYear].First());
            _playerBids = new Dictionary<string, int>();
        }


        private void TakeEvents()
        {
            foreach (var player in Players)
            {
                var toTakeEvents = Config.GameParametersByPlayersCount[Players.Count].EventsByPlayer;
                for (var i = 0; i < toTakeEvents; i++)
                {
                    player.Events.Add(new GameEvent(Probabilities.Pick(), Events.Pick()));
                    player.Events.Add(new GameEvent(Probabilities.Pick(), Events.Pick()));
                }
            }
        }

        public void FinishYear()
        {
            ApplicateEvents();
            CalculateMoney();
            PayCredits();
            _currentYear++;
        }

        private void PayCredits()
        {
            //TODO
        }

        private void CalculateMoney()
        {
            foreach (var company in Companies)
            {
                var income = company.MarketShare * Config.MoneyByMarketPart;
                var outcome = company.Manager.Bonus + company.RessourceDependencies.Sum(x => RessourcePrices[x]);
                company.Money += income;
                company.Money -= outcome;
            }
        }

        private void ApplicateEvents()
        {
            var config = Config.GameParametersByPlayersCount[Players.Count];
            var events = Players.SelectMany(x => x.Events.Pick().Take(config.EventsByPlayerYear));
            var eventToApply = events.OrderBy(x => x.Probability).Take(config.EventsToApplicateInTheEndOfYear);
            foreach (var gameEvent in eventToApply)
            {
                gameEvent.Action.Applicate(this);
            }
        }

        public void Next()
        {
            if (Equals(GameState, GameState.Pricing))
            {
                if (_playerBids.Count == Players.Count)
                {
                    var lowestPrice = _playerBids.Values.Min();
                    var highestPrice = _playerBids.Values.Max();
                    if (lowestPrice == highestPrice)
                    {
                        GameState = GameState.VotingForManager.Company(GameState.CurrentCompany);
                        return;
                    }

                    var sellers = _playerBids.Where(x => x.Value == lowestPrice).ToList();
                    var buyers = _playerBids.Where(x => x.Value == highestPrice).ToList();

                    if (sellers.Count == buyers.Count)
                    {
                        for (int i = 0; i < sellers.Count; i++)
                        {
                            var transactionPrice = (lowestPrice + highestPrice) / 2;
                            var sellerPlayer = Players.First(x => x.Name == sellers[i].Key);
                            var buyerPlayer = Players.First(x => x.Name == buyers[i].Key);

                            var action = sellerPlayer.Actions.Pick(x => x.Company.Name == GameState.CurrentCompany).First();
                            sellerPlayer.Money += transactionPrice;
                            buyerPlayer.Actions.Add(action);
                            buyerPlayer.Money -= transactionPrice;
                        }
                    }
                    else
                    {
                        GameState = GameState.ChoosingExchangePartner.Company(GameState.CurrentCompany);
                        return;
                    }
                }
                else
                {
                    throw new Exception("Not all players have set the price");
                }
            }
            else
            {

            }
        }
    }
}