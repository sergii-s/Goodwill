using System;

namespace Goodwill.Core.Events
{
    public class GameEventAction : IGameEventAction
    {
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