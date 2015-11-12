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
        Ressource[] Ressources { get; }
        int MaxRessourcePrice { get; }
        int MinRessourcePrice { get; }
        int InitialRessourcePrice { get; }
        List<Manager> Managers { get; }
        IGameEventAction[] Events { get; }
        int[] Percentages { get; }
        IDictionary<int, IParameters> GameParametersByPlayersCount { get; }
        IDictionary<int, string[]> CompanyEvaluatingOrderByYear { get; }
    }

    public interface IParameters
    {
        int EventsByPlayerYear { get; }
        int EventsByPlayer { get; }
        int EventsToApplicateInTheEndOfYear { get; }
    }

    public class Parameters : IParameters
    {
        public Parameters(int eventsByPlayerYear, int eventsByPlayer, int eventsToApplicateInTheEndOfYear)
        {
            EventsByPlayerYear = eventsByPlayerYear;
            EventsByPlayer = eventsByPlayer;
            EventsToApplicateInTheEndOfYear = eventsToApplicateInTheEndOfYear;
        }

        public int EventsByPlayerYear { get; }
        public int EventsByPlayer { get; }
        public int EventsToApplicateInTheEndOfYear { get; }
    }
}