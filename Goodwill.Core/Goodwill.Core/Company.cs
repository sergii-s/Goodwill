using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        public Manager(string name, int bonus, bool dividends, int promotion, int optimisation, int innovation, RessourceInfo innovationFrom, RessourceInfo innovationTo)
        {
            Name = name;
            Bonus = bonus;
            Dividends = dividends;
            Promotion = promotion;
            Optimisation = optimisation;
            Innovation = innovation;
            InnovationFrom = innovationFrom;
            InnovationTo = innovationTo;
        }

        public string Name { get;}
        public int Bonus { get; }
        public bool Dividends { get; }
        public int Promotion { get; }
        public int Optimisation { get; }
        public int Innovation { get; }
        public RessourceInfo InnovationFrom { get; }
        public RessourceInfo InnovationTo { get; }
    }
}