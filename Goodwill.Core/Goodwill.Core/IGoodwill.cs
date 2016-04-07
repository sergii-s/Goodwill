namespace Goodwill.Core
{
    public interface IGoodwill
    {
        Player AddPlayer(string playerName);
        void Start();
        GameInfo GetGameInfo();
    }
}