using System.Collections.Generic;

namespace Goodwill.Core
{
    public class Constants
    {
        public static readonly string Athena = "Athena";
        public static readonly string Mercury = "Mercury";
        public static readonly string Jupiter = "Jupiter";
    }

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
        public Manager(string name, int bonus)
        {
            Name = name;
            Bonus = bonus;
        }

        public string Name { get;}
        public int Bonus { get; }
    }
}