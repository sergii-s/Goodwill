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
            Ressources = new[] { Ressource.Coal, Ressource.Fuel, Ressource.Employee };
            InitialRessourcePrice = 5;
            MinRessourcePrice = 5;
            MaxRessourcePrice = 25;
            Managers = new List<Manager>
            {
                Build.Manager
                    .Named("Helene")
                    .WithInnovationLevel(2,Ressource.Employee,Ressource.Fuel)
                    .WithPromotionLevel(2)
                    .Create(),
                Build.Manager
                    .Named("Gaston")
                    .WithBonus(5)
                    .WithInnovationLevel(2,Ressource.Employee,Ressource.Coal)
                    .WithOptimisationLevel(2)
                    .WithPromotionLevel(1)
                    .WithDividends()
                    .Create(),
                Build.Manager
                    .Named("Alphonse")
                    .WithOptimisationLevel(3)
                    .WithPromotionLevel(1)
                    .WithDividends()
                    .Create(),
                Build.Manager
                    .Named("Ingrid")
                    .WithOptimisationLevel(2)
                    .WithPromotionLevel(2)
                    .WithDividends()
                    .Create(),
                Build.Manager
                    .Named("Colette")
                    .WithBonus(10)
                    .WithOptimisationLevel(1)
                    .WithPromotionLevel(3)
                    .WithDividends()
                    .Create(),
                Build.Manager
                    .Named("Edouard")
                    .WithBonus(5)
                    .WithInnovationLevel(3,Ressource.Fuel,Ressource.Employee)
                    .WithPromotionLevel(2)
                    .Create(),
                Build.Manager
                    .Named("Derek")
                    .WithBonus(10)
                    .WithInnovationLevel(1,Ressource.Coal,Ressource.Employee)
                    .WithPromotionLevel(3)
                    .Create(),
                Build.Manager
                    .Named("Boris")
                    .WithInnovationLevel(1,Ressource.Fuel,Ressource.Coal)
                    .WithOptimisationLevel(3)
                    .Create(),
                Build.Manager
                    .Named("Francois")
                    .WithBonus(5)
                    .WithInnovationLevel(3,Ressource.Coal,Ressource.Fuel)
                    .WithOptimisationLevel(1)
                    .WithPromotionLevel(1)
                    .Create(),
            };
            Events = new[]
            {
                Build.Event
                    .PriceChange(Ressource.Fuel, 10)
                    .Promotion(3)
                    .Create(),
                Build.Event
                    .Promotion(3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Coal, -10)
                    .Optimisation(3)
                    .Create(),
                Build.Event
                    .RessourceBonus(Ressource.Coal, 10)
                    .Innovation(2, 3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Fuel, -10)
                    .Optimisation(3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Employee, 10)
                    .Innovation(2)
                    .Create(),
                Build.Event
                    .RessourceBonus(Ressource.Fuel, 10)
                    .Optimisation(1)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Coal, -5)
                    .Innovation(2)
                    .Create(),
                Build.Event
                    .RessourceBonus(Ressource.Employee, 10)
                    .Promotion(3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Coal, 10)
                    .Optimisation(2)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Employee, -5)
                    .Innovation(1,3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Employee, 5)
                    .Optimisation(3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Fuel, -5)
                    .Promotion(1)
                    .Create(),
                Build.Event
                    .Scandal("Athena")
                    .Promotion(1)
                    .Create(),
                Build.Event
                    .Scandal("Jupiter")
                    .Innovation(3)
                    .Create(),
                Build.Event
                    .Scandal("Mercury")
                    .Promotion(2)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Fuel, 5)
                    .Innovation(1)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Employee, -10)
                    .Promotion(3)
                    .Create(),
                Build.Event
                    .PriceChange(Ressource.Coal, 5)
                    .Promotion(2)
                    .Create(),
                Build.Event
                    .PriceChangeAllRessources(5)
                    .Innovation(3)
                    .Create(),
                Build.Event
                    .PriceChangeAllRessources(-5)
                    .Innovation(3)
                    .Create(),
                Build.Event
                    .RessourceTax(Ressource.Fuel, 10)
                    .Innovation(2,3)
                    .Create(),
                Build.Event
                    .RessourceTax(Ressource.Employee, 10)
                    .Innovation(2,3)
                    .Create(),
                Build.Event
                    .RessourceTax(Ressource.Coal, 10)
                    .Promotion(2)
                    .Create(),
            };
            Percentages = new[]
            {
                0,
                5,
                10,
                15,
                20,
                25,
                30,
                35,
                40,
                45,
                50,
                55,
                60,
                65,
                70,
                75,
                80,
                85,
                90,
                95,
                100
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

        public int[] Percentages { get; }
        public int Years { get; }
        public int MoneyByMarketPart { get; }
        public Ressource[] Ressources { get; }
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