using System;

namespace Goodwill.Core.Events
{
    public class GameEventAction : IGameEventAction
    {
        public static IGameEventAction RessourcePriceMove(RessourceInfo ressource, int priceChange)
        {
            return new GameEventAction(goodwill =>
            {
                var newPrice = goodwill.RessourcePrices[ressource] + priceChange;
                newPrice = (newPrice > goodwill.Config.MaxRessourcePrice) ? goodwill.Config.MaxRessourcePrice : newPrice;
                newPrice = (newPrice < goodwill.Config.MinRessourcePrice) ? goodwill.Config.MinRessourcePrice : newPrice;
                goodwill.RessourcePrices[ressource] = newPrice;
            });
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