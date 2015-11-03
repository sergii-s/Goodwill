using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Goodwill.Core;
using Goodwill.Web.Models;
using static System.Int32;
using Player = Goodwill.Web.Models.Player;

namespace Goodwill.Web.Controllers
{
    public class GameController : ApiController
    {
        private static readonly IDictionary<string, Game> Games = new Dictionary<string, Game>();

        [HttpGet]
        public string Initialize()
        {
            var gameId = ShortGuid.NewShortGuid();
            var playerId = ShortGuid.NewShortGuid();

            Games[gameId] = new Game
            {
                Players = new Dictionary<string, Player>
                {
                    {playerId, new Player {Host = true, Connected = true, Humain = true, Name = "Boss"}}
                },
                Infos = new Dictionary<string, List<GameInfoForPlayer>>
                {
                    {playerId, new List<GameInfoForPlayer>()}
                }
            };

            var token = gameId + playerId;
            return token;
        }

        [HttpGet]
        public string InvitePlayer(string token, string email)
        {
            var playerToken = new Token(token);
            new TokenValidator(playerToken)
                .GameExists()
                .PlayerExists()
                .NotStarted()
                .IsHost();

            var game = Games[playerToken.GameId];
            game.AddPlayer(email);

            return "Invite is sent";
        }

        [HttpGet]
        public string AddComputer(string token)
        {
            var playerToken = new Token(token);
            new TokenValidator(playerToken)
                .GameExists()
                .PlayerExists()
                .NotStarted()
                .IsHost();

            var game = Games[playerToken.GameId];
            game.AddComputer();

            return "Added";
        }

        [HttpGet]
        public string Start(string token)
        {
            var playerToken = new Token(token);
            new TokenValidator(playerToken)
                .GameExists()
                .PlayerExists()
                .NotStarted()
                .IsHost();

            var game = Games[playerToken.GameId];
            game.Start();

            return "Success";
        }

        [HttpGet]
        public List<GameInfoForPlayer> Info(string token, string state)
        {
            var playerToken = new Token(token);
            new TokenValidator(playerToken)
                .GameExists()
                .PlayerExists()
                .NotStarted()
                .IsHost();

            var game = Games[playerToken.GameId];

            if (string.IsNullOrEmpty(state))
            {
                return game.Infos[playerToken.PlayerId];
            }

            var latestStateId = Parse(state);
            return game.Infos[playerToken.PlayerId].Where(x => x.GameStateId > latestStateId).ToList();
        }

        // GET api/game
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        private class TokenValidator
        {
            private readonly Token _playerToken;

            public TokenValidator(Token playerToken)
            {
                _playerToken = playerToken;
            }

            public TokenValidator GameExists()
            {
                if (!Games.ContainsKey(_playerToken.GameId))
                {
                    throw new Exception("Game doesn't exist");
                }
                return this;
            }

            public TokenValidator PlayerExists()
            {
                var game = Games[_playerToken.GameId];
                if (!game.Players.ContainsKey(_playerToken.PlayerId))
                {
                    throw new Exception("Player token is not valid");
                }
                return this;
            }

            public TokenValidator NotStarted()
            {
                var game = Games[_playerToken.GameId];
                if (game.Started)
                {
                    throw new Exception("The game is already started");
                }
                return this;
            }

            public void IsHost()
            {
                var game = Games[_playerToken.GameId];
                var player = game.Players[_playerToken.PlayerId];
                if (!player.Host)
                {
                    throw new Exception("Not a host");
                }
            }
        }
    }
}
