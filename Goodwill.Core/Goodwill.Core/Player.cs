using System.Collections.Generic;
using Goodwill.Core.Events;

namespace Goodwill.Core
{
    public class Player
    {
        public string Name { get; set; }

        public int Money { get; set; }

        public List<CompanyAction> Actions { get; set; }
        public List<GameEvent> Events { get; set; } = new List<GameEvent>();
    }

    public class CompanyAction
    {
        public Company Company { get; set; }
    }
}