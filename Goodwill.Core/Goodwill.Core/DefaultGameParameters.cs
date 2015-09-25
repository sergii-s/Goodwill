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
            Companies = new[] {"Athena", "Mercury", "Jupiter"};
        }

        public int Years { get; }
        public int InitialCompanyMoney { get; }
        public int BonusCompanyMoneyPerMarketPart { get; }
        public int InitialPlayerMoney { get; }
        public int BonusPlayerMoneyPerMissingAction { get; }
        public int ActionsByCompany { get; }
        public int MarketPartDivider { get; }
        public string[] Companies { get; }
    }
}