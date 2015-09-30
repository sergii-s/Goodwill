namespace Goodwill.Core
{
    public interface IGameInitializer
    {
        void InitializeGame(Goodwill goodwill, IGameParameters config);
        void InitializeEvents(Goodwill goodwill, IGameParameters config);
    }
}