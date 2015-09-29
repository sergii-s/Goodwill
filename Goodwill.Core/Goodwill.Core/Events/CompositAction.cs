using System.Collections.Generic;

namespace Goodwill.Core.Events
{
    public class CompositAction : List<IGameEventAction>, IGameEventAction
    {
        private CompositAction(IEnumerable<IGameEventAction> actions) : base(actions)
        {
        }

        public static IGameEventAction Create(params IGameEventAction[] actions)
        {
            return new CompositAction(actions);
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