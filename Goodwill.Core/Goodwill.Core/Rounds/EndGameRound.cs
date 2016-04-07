namespace Goodwill.Core.Rounds
{
    public class EndGameRound : IGameRound
    {
        private readonly Goodwill _goodwill;

        public EndGameRound(Goodwill goodwill)
        {
            _goodwill = goodwill;
        }

        public void FinishRound()
        {

        }
    }
}