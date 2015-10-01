using System.Collections.Generic;
using Goodwill.Core.Events;

namespace Goodwill.Core
{
    public class DefaultGameParameters : IGameParameters
    {
        public DefaultGameParameters()
        {
            InitialPlayerMoney = 20;
            InitialCompanyMoney = 100;
            BonusCompanyMoneyPerMarketPart = 10;
            BonusPlayerMoneyPerMissingAction = 13;
            ActionsByCompany = 10;
            MarketPartDivider = 5;
            Years = 6;
            Companies = new[] { "Athena", "Mercury", "Jupiter" };
            MoneyByMarketPart = 1;
            Ressources = new[] { RessourceInfo.Coal, RessourceInfo.Fuel, RessourceInfo.Employee };
            InitialRessourcePrice = 5;
            MinRessourcePrice = 5;
            MaxRessourcePrice = 25;
            Managers = new List<Manager>
            {
                new Manager("Francois", 0),
                new Manager("Derec", 5),
                new Manager("Helene", 10),
                new Manager("x1", 0),
                new Manager("x2", 0),
                new Manager("x3", 0),
                new Manager("x4", 0),
                new Manager("x5", 0),
                new Manager("x6", 0)
            };
            Events = new[]
            {
                //TODO compose all events
                GameEventAction.RessourcePriceMove(RessourceInfo.Coal, 5),
                GameEventAction.RessourcePriceMove(RessourceInfo.Coal, 10),
                GameEventAction.RessourcePriceMove(RessourceInfo.Coal, -5),
                GameEventAction.RessourcePriceMove(RessourceInfo.Coal, -10),
                GameEventAction.RessourcePriceMove(RessourceInfo.Fuel, 5),
                GameEventAction.RessourcePriceMove(RessourceInfo.Fuel, 10),
                GameEventAction.RessourcePriceMove(RessourceInfo.Fuel, -5),
                GameEventAction.RessourcePriceMove(RessourceInfo.Fuel, -10),
                GameEventAction.RessourcePriceMove(RessourceInfo.Employee, 5),
                GameEventAction.RessourcePriceMove(RessourceInfo.Employee, 10),
                GameEventAction.RessourcePriceMove(RessourceInfo.Employee, -5),
                GameEventAction.RessourcePriceMove(RessourceInfo.Employee, -10),
            };
            GameParametersByPlayersCount = new Dictionary<int, IParameters>()
            {
                //TODO correct config
                {2, new Parameters(4, 4, 4)},
                {3, new Parameters(4, 4, 4)},
                {4, new Parameters(2, 2, 4)},
                {5, new Parameters(2, 2, 4)},
                {6, new Parameters(2, 2, 4)},
                {7, new Parameters(2, 2, 4)},
                {8, new Parameters(2, 2, 4)},
            };
            CompanyEvaluatingOrderByYear = new Dictionary<int, string[]>()
            {
                {0, new[] {"Athena", "Mercury", "Jupiter"}},
                {1, new[] {"Athena", "Mercury", "Jupiter"}},
                {2, new[] {"Athena", "Mercury", "Jupiter"}},
                {3, new[] {"Athena", "Mercury", "Jupiter"}},
                {4, new[] {"Athena", "Mercury", "Jupiter"}},
                {5, new[] {"Athena", "Mercury", "Jupiter"}},
                {6, new[] {"Athena", "Mercury", "Jupiter"}},
                {7, new[] {"Athena", "Mercury", "Jupiter"}},
            };
        }

        public int Years { get; }
        public int MoneyByMarketPart { get; }
        public RessourceInfo[] Ressources { get; }
        public int MaxRessourcePrice { get; }
        public int MinRessourcePrice { get; }
        public int InitialRessourcePrice { get; }
        public List<Manager> Managers { get; }
        public int InitialCompanyMoney { get; }
        public int BonusCompanyMoneyPerMarketPart { get; }
        public int InitialPlayerMoney { get; }
        public int BonusPlayerMoneyPerMissingAction { get; }
        public int ActionsByCompany { get; }
        public int MarketPartDivider { get; }
        public string[] Companies { get; }
        public IGameEventAction[] Events { get; }
        public IDictionary<int, IParameters> GameParametersByPlayersCount { get; }
        public IDictionary<int, string[]> CompanyEvaluatingOrderByYear { get; }
    }
}