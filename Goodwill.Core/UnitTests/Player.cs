using System.Collections.Generic;

namespace UnitTests
{
    public class Player
    {
        public string Name { get; set; }

        public int Money { get; set; }

        public List<CompanyAction> Actions { get; set; }
    }

    public class CompanyAction
    {
        public Company Company { get; set; }
    }
}