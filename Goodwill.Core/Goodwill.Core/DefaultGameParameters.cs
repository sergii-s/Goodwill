namespace UnitTests
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
        }

        public int Years { get; }
        public int MoneyByMarketPart { get; }
        public RessourceInfo[] Ressources { get; }
        public int InitialRessourcePrice { get; }
        public int InitialCompanyMoney { get; }
        public int BonusCompanyMoneyPerMarketPart { get; }
        public int InitialPlayerMoney { get; }
        public int BonusPlayerMoneyPerMissingAction { get; }
        public int ActionsByCompany { get; }
        public int MarketPartDivider { get; }
        public string[] Companies { get; }
    }
}