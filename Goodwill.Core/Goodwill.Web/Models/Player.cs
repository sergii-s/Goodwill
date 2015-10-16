using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Goodwill.Web.Models
{
    public class Player
    {
        public bool Host { get; set; }
        public bool Connected { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}