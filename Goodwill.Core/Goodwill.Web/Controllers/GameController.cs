using System;
using System.Collections.Generic;
using System.Web.Http;
using Goodwill.Web.Models;

namespace Goodwill.Web.Controllers
{
    public class GameController : ApiController
    {
        [HttpGet]
        public string Initialize()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpGet]
        public string Start(string gameId, Player[] players)
        {
            return "Game started";
        }

        // GET api/game
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        
    }
}
