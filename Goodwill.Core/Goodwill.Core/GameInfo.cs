using System.Collections.Generic;
using System.Linq;
using Goodwill.Core.Events;

namespace Goodwill.Core
{
    public class GameInfo
    {
        public int CurrentYear { get; set; }
        public int TotalYears { get; set; }
        public Dictionary<string, CompanyInfo> Companies { get; set; }
        public Dictionary<Ressource, int> Ressources { get; set; }
        public GameState State { get; set; }
        public List<PlayerInfo> Players { get; set; }
        public IDictionary<string, PlayerInfo> PlayersDictionary
        {
            get { return Players.ToDictionary(x => x.Name, x => x); }
        }
    }

    public class PlayerInfo
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public List<ActionInfo> Actions { get; set; }
        public List<GameEvent> Events { get; set; }
    }

    public class ActionInfo
    {
        public string Company { get; set; }
    }
}