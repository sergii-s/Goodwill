using System;
using System.Collections.Generic;
using System.Linq;
using Goodwill.Core.Events;
using Goodwill.Core.Rounds;

namespace Goodwill.Core
{
    public class Goodwill : IGoodwill
    {
        public IGameParameters Config { get; }
        private readonly IGameInitializer _gameInitializer;
        private LinkedListNode<IGameRound> _currentRound;
        public int CurrentYear = 0;

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
        public List<Manager> AvailableManagers { get; } = new List<Manager>();
        public Deck<IGameEventAction> Events { get; set; }
        public Deck<int> Probabilities { get; set; }
        public IDictionary<Ressource, int> RessourcePrices { get; } = new Dictionary<Ressource, int>();
        public IGameRound CurrentRound => _currentRound.Value;

        private LinkedList<IGameRound> _gameRounds;

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
            _gameRounds = new LinkedList<IGameRound>(AllGameRounds());
            _currentRound = _gameRounds.First;
        }

        public GameInfo GetGameInfo()
        {
            return new GameInfo
            {
                CurrentYear = CurrentYear,
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
                    Events = x.Events.ToList()
                }).ToList(),
                AvailableManagers = AvailableManagers.ToList()
            };
        }
        
        public IGameRound NextRound()
        {
            _currentRound.Value.FinishRound();
            _currentRound = _currentRound.Next;
            return _currentRound.Value;
        }

        private IEnumerable<IGameRound> AllGameRounds()
        {
            for (var i = 0; i < Config.Years; i++)
            {
                yield return new BeginYearRound(this);
                foreach (var company in Config.CompanyEvaluatingOrderByYear[i])
                {
                    yield return new BiddingRound(this, company);
                    yield return new VoteManagerRound(this, company);
                    yield return new BiddingRound(this, company);
                }
                yield return new EndYearRound(this);
            }
            yield return new EndGameRound(this);
        }
    }
}