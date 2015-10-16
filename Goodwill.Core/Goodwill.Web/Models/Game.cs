using System.Collections.Generic;
using System.Linq;

namespace Goodwill.Web.Models
{
    public class Game
    {
        private Core.Goodwill _game;
        public IDictionary<string, Player> Players { get; set; }
        public bool Started { get; set; }

        public void Start()
        {
            var notConnected = Players.Where(x => !x.Value.Connected);
            foreach (var keyValuePair in notConnected)
            {
                Players.Remove(keyValuePair.Key);
            }

            _game = new Core.Goodwill();
            foreach (var player in Players)
            {
                _game.AddPlayer(player.Value.Name);
            }
            _game.Start();
        }
    }
}