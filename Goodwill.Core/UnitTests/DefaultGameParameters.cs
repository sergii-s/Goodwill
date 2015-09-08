namespace UnitTests
{
    public class DefaultGameParameters : IGameParameters
    {
        public DefaultGameParameters()
        {
            InitialPlayerMoney = 20;
            InitialCompanyMoney = 100;
            BonusCompanyMoneyPerMarketPart = 10;
            BonusPlayerMoneyPerAction = 13;
            ActionsByCompany = 10;
            MarketPartDivider = 5;
            Companies = new[] {"Athena", "Mercury", "Jupiter"};
        }

        public int InitialCompanyMoney { get; private set; }
        public int BonusCompanyMoneyPerMarketPart { get; private set; }
        public int InitialPlayerMoney { get; private set; }
        public int BonusPlayerMoneyPerAction { get; private set; }
        public int ActionsByCompany { get; private set; }
        public int MarketPartDivider { get; private set; }
        public string[] Companies { get; private set; }
    }
}