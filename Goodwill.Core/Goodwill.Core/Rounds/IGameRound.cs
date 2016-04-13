using System.Security.Cryptography.X509Certificates;

namespace Goodwill.Core.Rounds
{
    public interface IGameRound
    {
        void FinishRound();
        GameState State { get; }
    }
}