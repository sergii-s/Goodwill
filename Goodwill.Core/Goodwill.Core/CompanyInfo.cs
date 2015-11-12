using System.Collections.Generic;

namespace Goodwill.Core
{
    public class CompanyInfo
    {
        public string Name { get; set; }
        public int Money { get; set; }
        public int MarketShare { get; set; }
        public Manager Manager { get; set; }
        public List<RessourceInfo> RessourceDependencies { get; set; }
    }
}