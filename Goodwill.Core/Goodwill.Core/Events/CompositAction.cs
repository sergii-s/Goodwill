using System.Collections.Generic;

namespace Goodwill.Core.Events
{
    public class CompositAction : List<IGameEventAction>, IGameEventAction
    {
        public CompositAction(IEnumerable<IGameEventAction> actions) : base(actions)
        {
        }
        
        public void Applicate(Goodwill game)
        {
            foreach (var eventAction in this)
            {
                eventAction.Applicate(game);
            }
        }
    }
}