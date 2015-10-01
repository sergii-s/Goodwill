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

        public Deck<GameEvent> Events { get; set; }

        public IDictionary<RessourceInfo, int> RessourcePrices { get; } = new Dictionary<RessourceInfo, int>();
        

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
                    RessourceDependencies = x.RessourceDependencies
                }).ToDictionary(x => x.Name, x => x),
                Ressources = RessourcePrices.ToDictionary(x => x.Key, x => x.Value)
            };
        }

        public void SetPrice(string player, string company, int price)
        {
            throw new NotImplementedException();
        }

        public void VoteManager(string player, string company, string manager)
        {
            throw new NotImplementedException();
        }

        private void BeginYear()
        {
            _gameInitializer.InitializeEvents(this, Config);
            DistributeEvents();
        }

        private void DistributeEvents()
        {
            foreach (var player in Players)
            {
                player.Events = Events.Pick(2);
            }
        }

        public void FinishYear()
        {
            ApplicateEvents();
            CalculateMoney();
            _currentYear++;
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
            var events = Players.SelectMany(x => x.Events);
            var eventToApply = events.OrderBy(x => x.Probability).Take(4);
            foreach (var gameEvent in eventToApply)
            {
                gameEvent.Action.Applicate(this);
            }
        }
    }

    public interface IGoodwill
    {
        Player AddPlayer(string playerName);
        void Start();
        GameInfo GetGameInfo();
        void SetPrice(string player, string company, int price);
        void VoteManager(string player, string company, string manager);
    }

    public class GameInfo
    {
        public int CurrentYear { get; set; }
        public int TotalYears { get; set; }
        public Dictionary<string, CompanyInfo> Companies { get; set; }
        public Dictionary<RessourceInfo, int> Ressources { get; set; }

        public GameState State { get; set; }
    }
    
    public class GameState
    {
    }

    public class EvaluatingPriceState : GameState
    {
        private EvaluatingPriceState(string company)
        {
        }

        public static GameState For(string company)
        {
            return new EvaluatingPriceState(company);
        }
    }

    public class CompanyInfo
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public int MarketShare { get; set; }
        public ManagerInfo Manager { get; set; }
        public List<RessourceInfo> RessourceDependencies { get; set; }
    }

    public enum RessourceInfo
    {
        Coal, Fuel, Employee
    }

    public class ManagerInfo
    {
    }
}