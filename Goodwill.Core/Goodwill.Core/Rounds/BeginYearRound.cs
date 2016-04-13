using Goodwill.Core.Events;

namespace Goodwill.Core.Rounds
{
    public class BeginYearRound : IGameRound
    {
        private readonly Goodwill _goodwill;

        public BeginYearRound(Goodwill goodwill)
        {
            _goodwill = goodwill;
        }

        public void FinishRound()
        {
            foreach (var player in _goodwill.Players)
            {
                var toTakeEvents = _goodwill.Config.GameParametersByPlayersCount[_goodwill.Players.Count].EventsByPlayer;
                for (var i = 0; i < toTakeEvents; i++)
                {
                    player.Events.Add(new GameEvent(_goodwill.Probabilities.Pick(), _goodwill.Events.Pick()));
                    player.Events.Add(new GameEvent(_goodwill.Probabilities.Pick(), _goodwill.Events.Pick()));
                }
            }
        }

        public GameState State => new GameState {Round = EGameRound.BeginYear};
    }
}