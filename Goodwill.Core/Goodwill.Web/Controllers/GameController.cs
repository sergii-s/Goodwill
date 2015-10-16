using System;
using System.Collections.Generic;
using System.Web.Http;
using Goodwill.Web.Models;

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
                    {playerId, new Player {Host = true, Connected = true}}
                }
            };

            var token = gameId + playerId;
            return token;
        }

        [HttpGet]
        public string InvitePlayer(string tokenString, string playerEmail)
        {
            var token = new Token(tokenString);
            if (!Games.ContainsKey(token.GameId))
            {
                throw new Exception("Game doesnt exist");
            }
            var game = Games[token.GameId];
            if (!game.Players.ContainsKey(token.PlayerId))
            {
                throw new Exception("Player token is not valid");
            }
            if (!game.Started)
            {
                throw new Exception("You cannot invite players when the game is already started");
            }
            var player = game.Players[token.PlayerId];
            if (!player.Host)
            {
                throw new Exception("Only host can invite other players");
            }

            var playerId = ShortGuid.NewShortGuid();
            game.Players[playerId] = new Player
            {
                Connected = false,
                Email = playerEmail
            };

            return "Invite is sent";
        }

        [HttpGet]
        public string Start(string tokenString)
        {
            var token = new Token(tokenString);
            if (!Games.ContainsKey(token.GameId))
            {
                throw new Exception("Game doesnt exist");
            }
            var game = Games[token.GameId];
            if (!game.Players.ContainsKey(token.PlayerId))
            {
                throw new Exception("Player token is not valid");
            }
            if (!game.Started)
            {
                throw new Exception("Game is already started");
            }
            var player = game.Players[token.PlayerId];
            if (!player.Host)
            {
                throw new Exception("Only host can start game");
            }

            game.Start();

            return "Success";
        }

        // GET api/game
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
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
