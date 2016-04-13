using System;
using System.Linq;

namespace Goodwill.Core.Rounds
{
    public class EndYearRound : IGameRound
    {
        private readonly Goodwill _goodwill;

        public EndYearRound(Goodwill goodwill)
        {
            _goodwill = goodwill;
        }

        public void FinishRound()
        {
            ApplicateEvents();
            CalculateMoney();
            PayCredits();
            _goodwill.CurrentYear++;
        }

        public GameState State => new GameState { Round = EGameRound.EndYear };


        private void PayCredits()
        {
            throw new NotImplementedException();
        }

        private void CalculateMoney()
        {
            foreach (var company in _goodwill.Companies)
            {
                var income = company.MarketShare * _goodwill.Config.MoneyByMarketPart;
                var outcome = company.Manager.Bonus + company.RessourceDependencies.Sum(x => _goodwill.RessourcePrices[x]);
                company.Money += income;
                company.Money -= outcome;
            }
        }

        private void ApplicateEvents()
        {
            var config = _goodwill.Config.GameParametersByPlayersCount[_goodwill.Players.Count];
            var events = _goodwill.Players.SelectMany(x => x.Events.Pick().Take(config.EventsByPlayerYear));
            var eventToApply = events.OrderBy(x => x.Probability).Take(config.EventsToApplicateInTheEndOfYear);
            foreach (var gameEvent in eventToApply)
            {
                gameEvent.Action.Applicate(_goodwill);
            }
        }
    }
}