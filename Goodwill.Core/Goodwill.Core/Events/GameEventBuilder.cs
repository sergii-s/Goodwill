using System.Collections.Generic;
using System.Linq;

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
            })
            {
                Name = $"Ressource{ressource}Price{priceChange}"
            });
            return this;
        }

        public GameEventBuilder Promotion(params int[] i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = string.Join(",", i.Select(x => $"Promotion{x}"))
            });
            return this;
        }

        public GameEventBuilder Innovation(params int[] i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = string.Join(",", i.Select(x => $"Innovation{x}"))
            });
            return this;
        }

        public GameEventBuilder Optimisation(params int[] i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = string.Join(",", i.Select(x=>$"Optimisation{x}"))
            });
            return this;
        }

        public GameEventBuilder RessourceBonus(Ressource ressource, int i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = $"RessourceBonus{ressource}Bonus{i}"
            });
            return this;
        }

        public GameEventBuilder RessourceTax(Ressource ressource, int i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = $"RessourceTax{ressource}Tax{i}"
            });
            return this;
        }

        public GameEventBuilder PriceChangeAllRessources(int i)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = $"AllRessourcesPriceChange{i}"
            });
            return this;
        }

        public GameEventBuilder Scandal(string company)
        {
            _actions.Add(new GameEventAction(goodwill =>
            {
                //TODO
            })
            {
                Name = $"Scandal{company}"
            });
            return this;
        }
    }
}