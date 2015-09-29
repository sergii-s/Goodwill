namespace Goodwill.Core.Events
{
    public class GameEvent
    {
        public GameEvent(int probability, GameEventAction action)
        {
            Probability = probability;
            Action = action;
        }

        public int Probability { get; }
        public IGameEventAction Action { get;}
    }
}