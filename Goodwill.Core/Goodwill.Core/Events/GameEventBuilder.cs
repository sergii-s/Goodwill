using System.Collections.Generic;

namespace Goodwill.Core.Events
{
    public class GameEventBuilder
    {
        private readonly List<IGameEventAction> _actions = new List<IGameEventAction>();

        public IGameEventAction Create()
        {
            return new CompositAction(_actions);
        }

        public GameEventBuilder PriceChange(Ressource ressource, int priceChange)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                var newPrice = goodwill.RessourcePrices[ressource] + priceChange;
                newPrice = (newPrice > goodwill.Config.MaxRessourcePrice) ? goodwill.Config.MaxRessourcePrice : newPrice;
                newPrice = (newPrice < goodwill.Config.MinRessourcePrice) ? goodwill.Config.MinRessourcePrice : newPrice;
                goodwill.RessourcePrices[ressource] = newPrice;
            }));
            return this;
        }

        public GameEventBuilder Promotion(params int[] i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }

        public GameEventBuilder Innovation(params int[] i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }

        public GameEventBuilder Optimisation(params int[] i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }

        public GameEventBuilder RessourceBonus(Ressource ressource, int i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }

        public GameEventBuilder RessourceTax(Ressource ressource, int i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }

        public GameEventBuilder PriceChangeAllRessources(int i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }

        public GameEventBuilder Scandal(string company)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            }));
            return this;
        }
    }
}