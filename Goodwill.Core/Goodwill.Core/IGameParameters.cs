using System.Collections.Generic;

namespace Goodwill.Core
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
        int Years{ get; }
        int MoneyByMarketPart { get; }
        RessourceInfo[] Ressources { get; }
        int InitialRessourcePrice { get; }
        List<Manager> Managers { get; }
    }
}