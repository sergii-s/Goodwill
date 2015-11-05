using System.Collections.Generic;
using System.Linq;
using Goodwill.Core;
using Goodwill.Web.Controllers;

namespace Goodwill.Web.Models
{
    public class Game
    {
        private static readonly string[] Comuters = { "Momo", "Alex", "Mathieu4f" };

        private Core.Goodwill _game;
        public IDictionary<string, Player> Players { get; set; }
        public bool Started { get; set; }
        public Dictionary<string, List<GameInfoForPlayer>> Infos { get; set; }

        public void AddPlayer(string email)
        {
            var playerId = ShortGuid.NewShortGuid();
            Players[playerId] = new Player
            {
                PlayerPublicId = Players.Count,
                Connected = false,
                Humain = true,
                Email = email
            };
            Infos[playerId] = new List<GameInfoForPlayer>();
            AddSnapshotInfo();
        }

        public void AddComputer()
        {
            var playerId = ShortGuid.NewShortGuid();
            Players[playerId] = new Player
            {
                PlayerPublicId = Players.Count,
                Connected = true,
                Humain = false,
                Name = Comuters.Random()
            };
            AddSnapshotInfo();
        }

        public void ConnectPlayer(string token)
        {
            Players[token].Connected = true;
            AddSnapshotInfo();
        }

        private void AddSnapshotInfo()
        {
            foreach (var player in Players.Where(x => x.Value.Connected && x.Value.Humain))
            {
                Infos[player.Key].Add(new GameInfoForPlayer
                {
                    GameStateId = Infos[player.Key].Count,
                    Name = Players[player.Key].Name,
                    Started = Started,
                    Players = Players.Where(x => x.Key != player.Key).Select(x => new PlayerPublicInfo
                    {
                        PlayerPublicId = x.Value.PlayerPublicId,
                        Name = x.Value.Name,
                        Humain = x.Value.Humain,
                        Connected = x.Value.Connected,
                        Host = x.Value.Host
                    }).ToList()
                });
            }
        }

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

    public class PlayerPublicInfo
    {
        public int PlayerPublicId { get; set; }
        public string Name { get; set; }
        public bool Humain { get; set; }
        public bool Connected { get; set; }
        public bool Host { get; set; }        
    }

    public class GameInfoForPlayer
    {
        public string Name { get; set; }
        public bool Started { get; set; }
        public List<PlayerPublicInfo> Players { get; set; }
        public int GameStateId { get; set; }
    }
}