using System.Collections.Generic;
using Goodwill.Core.Events;

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
        int Years { get; }
        int MoneyByMarketPart { get; }
        RessourceInfo[] Ressources { get; }
        int MaxRessourcePrice { get; }
        int MinRessourcePrice { get; }
        int InitialRessourcePrice { get; }
        List<Manager> Managers { get; }
        IGameEventAction[] Events { get; }
    }
}