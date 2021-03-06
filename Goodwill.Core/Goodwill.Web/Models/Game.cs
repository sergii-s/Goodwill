﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Goodwill.Core;
using Goodwill.Core.Rounds;
using Goodwill.Web.Controllers;

namespace Goodwill.Web.Models
{
    public class Game
    {
        private readonly TimeSpan _biddingTimeout = TimeSpan.FromMinutes(1);
        private readonly TimeSpan _votingTimeout = TimeSpan.FromMinutes(1);

        private readonly Deck<string> _computers = new List<string>{ "Julien", "Jeremie", "Momo", "Alex", "Mathieu4f" }.Shuffle();

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
                Name = _computers.Pick()
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
                var gameInfoForPlayer = new GameInfoForPlayer
                {
                    GameStateId = Infos[player.Key].Count,
                    Name = Players[player.Key].Name,
                    Started = Started,
                    Players = Players.Where(x => x.Key != player.Key).Select(x => new PlayerPublicInfo
                    {
                        PlayerPublicId = x.Value.PlayerPublicId,
                        Name = x.Value.Name,
                        Email = x.Value.Email,
                        Humain = x.Value.Humain,
                        Connected = x.Value.Connected,
                        Host = x.Value.Host
                    }).ToList()
                };
                if (Started)
                {
                    var gameInfo = _game.GetGameInfo();
                    gameInfoForPlayer.GameState = new GameStateInfo
                    {
                        State = gameInfo.GameState.Round.ToString(),
                        Company = gameInfo.GameState.Company
                    };
                    gameInfoForPlayer.GameInfo = gameInfo.PlayersDictionary[Players[player.Key].Name];
                    gameInfoForPlayer.Companies = gameInfo.Companies.Values.ToList();
                    gameInfoForPlayer.AvailableManagers = gameInfo.AvailableManagers;
                    gameInfoForPlayer.Ressources = gameInfo.Ressources.Select(x => new PricedRessource
                    {
                        Ressource = x.Key,
                        RessourceName = x.Key.ToString(),
                        Price = x.Value
                    }).ToList();
                    foreach (var playerPublicInfo in gameInfoForPlayer.Players)
                    {
                        playerPublicInfo.GameInfo = gameInfo.PlayersDictionary[playerPublicInfo.Name];
                    }
                }
                Infos[player.Key].Add(gameInfoForPlayer);
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

            Started = true;

            AddSnapshotInfo();
            ContinueGame().Start();
        }

        private async Task ContinueGame()
        {
            var currentRound = _game.CurrentRound;
            if (currentRound is BiddingRound)
            {
                await Task.Delay(_biddingTimeout);
            }
            if (currentRound is VoteManagerRound)
            {
                await Task.Delay(_votingTimeout);
            }
            _game.NextRound();
            AddSnapshotInfo();
            if (_game.CurrentRound == null)
            {
                return;
            }
            await ContinueGame();
        }

        public void SetPrice(string playerId, int price)
        {
            var currentRound = _game.CurrentRound as BiddingRound;
            if (currentRound != null)
            {
                currentRound.SetPrice(Players[playerId].Name, price);
                AddSnapshotInfo();
            }
        }

        public void VoteManager(string playerId, int price)
        {
            var currentRound = _game.CurrentRound as BiddingRound;
            if (currentRound != null)
            {
                currentRound.SetPrice(Players[playerId].Name, price);
                AddSnapshotInfo();
            }
        }
    }

    public class GameStateInfo
    {
        public string State { get; set; }
        public string Company { get; set; }
    }

    public class PlayerPublicInfo
    {
        public int PlayerPublicId { get; set; }
        public string Name { get; set; }
        public bool Humain { get; set; }
        public bool Connected { get; set; }
        public bool Host { get; set; }
        public string Email { get; set; }
        public PlayerInfo GameInfo { get; set; }
    }

    public class GameInfoForPlayer
    {
        public string Name { get; set; }
        public bool Started { get; set; }
        public List<PlayerPublicInfo> Players { get; set; }
        public int GameStateId { get; set; }
        public List<CompanyInfo> Companies { get; set; }
        public PlayerInfo GameInfo { get; set; }
        public List<Manager> AvailableManagers { get; set; }
        public List<PricedRessource> Ressources { get; set; }
        public GameStateInfo GameState { get; set; }
    }

    public class PricedRessource
    {
        public Ressource Ressource { get; set; }
        public string RessourceName { get; set; }
        public int Price { get; set; }
    }
}