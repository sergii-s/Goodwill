using System.Collections.Generic;

namespace UnitTests
{
    public interface IGameParameters
    {
        int InitialCompanyMoney { get; }
        int BonusCompanyMoneyPerMarketPart { get; }
        int InitialPlayerMoney { get; }
        int BonusPlayerMoneyPerMissingAction { get; }
        int ActionsByCompany { get; }
        int MarketPartDivider { get; }
        string[] Companies { get; }
    }
}