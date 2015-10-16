using System.Collections.Generic;

namespace Goodwill.Web.Models
{
    public class Game
    {
        public IDictionary<string, Player> Players { get; set; }
        public bool Started { get; set; }
    }
}