using System;
using System.Collections.Generic;
using System.Web.Http;
using Goodwill.Core;
using Goodwill.Web.Models;
using Player = Goodwill.Web.Models.Player;

namespace Goodwill.Web.Controllers
{
    public class GameController : ApiController
    {
        private static readonly IDictionary<string, Game> Games = new Dictionary<string, Game>();
        private static readonly string[] Comuters = { "1", "2", "3" };

        [HttpGet]
        public string Initialize()
        {
            var gameId = ShortGuid.NewShortGuid();
            var playerId = ShortGuid.NewShortGuid();

            Games[gameId] = new Game
            {
                Players = new Dictionary<string, Player>
                {
                    {playerId, new Player {Host = true, Connected = true}}
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
            var playerId = ShortGuid.NewShortGuid();
            game.Players[playerId] = new Player
            {
                Connected = false,
                Email = email
            };

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
            var playerId = ShortGuid.NewShortGuid();
            game.Players[playerId] = new Player
            {
                Connected = true,
                Name = Comuters.Random()
            };

            return "Invite is sent";
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
                if (!game.Started)
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
