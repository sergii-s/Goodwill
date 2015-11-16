namespace Goodwill.Core
{
    public enum Ressource
    {
        Coal, Fuel, Employee
    }

    public class RessourceInfo
    {
        public int Index { get; set; }
        public Ressource Ressource { get; set; }
        public string RessourceName { get; set; }
    }
}