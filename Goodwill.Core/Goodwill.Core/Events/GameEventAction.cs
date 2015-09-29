using System;

namespace Goodwill.Core.Events
{
    public class GameEventAction : IGameEventAction
    {
        public static IGameEventAction RessourcePriceMove(RessourceInfo ressource, int priceChange)
        {
            return new GameEventAction(goodwill => goodwill.RessourcePrices[ressource] += priceChange);
        }

        private readonly Action<Goodwill> _action;

        public GameEventAction(Action<Goodwill> action)
        {
            _action = action;
        }

        public void Applicate(Goodwill game)
        {
            _action(game);
        }
    }
}