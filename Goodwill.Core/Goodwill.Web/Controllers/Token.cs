namespace Goodwill.Web.Controllers
{
    public class Token
    {
        public Token(string token)
        {
            GameId = token.Substring(0, 10);
            PlayerId = token.Substring(10, 10);
        }

        public string GameId { get; }
        public string PlayerId { get; }

        public override string ToString()
        {
            return GameId + PlayerId;
        }
    }
}