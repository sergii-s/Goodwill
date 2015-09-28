using System.Collections.Generic;
using Microsoft.SqlServer.Server;

namespace UnitTests
{
    public class Company
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public int MarketShare { get; set; }
        public List<CompanyAction> Actions { get; set; }
        public List<RessourceInfo> RessourceDependencies { get; set; }
        public Manager Manager { get; set; }
    }

    public class Manager
    {
        public string Name { get; set; }
        public int Bonus { get; set; }
    }
}