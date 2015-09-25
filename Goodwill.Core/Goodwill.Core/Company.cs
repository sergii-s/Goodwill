using System.Collections.Generic;

namespace UnitTests
{
    public class Company
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public int MarketShare { get; set; }
        public List<CompanyAction> Actions { get; set; }
    }
}