namespace Goodwill.Core
{
    public class ManagerBuilder
    {
        private string _name;
        private int _bonus;
        private bool _dividends;
        private int _promotion;
        private int _optimisation;
        private int _innovation;
        private RessourceInfo _innovateFrom;
        private RessourceInfo _innovateTo;

        public ManagerBuilder Named(string name)
        {
            _name = name;
            return this;
        }

        public ManagerBuilder WithBonus(int bonus)
        {
            _bonus = bonus;
            return this;
        }

        public ManagerBuilder WithDividends()
        {
            _dividends = true;
            return this;
        }

        public ManagerBuilder WithPromotionLevel(int level)
        {
            _promotion = level;
            return this;
        }
        public ManagerBuilder WithOptimisationLevel(int level)
        {
            _optimisation = level;
            return this;
        }
        public ManagerBuilder WithInnovationLevel(int level, RessourceInfo ressourceFrom, RessourceInfo ressourceTo)
        {
            _innovation = level;
            _innovateFrom = ressourceFrom;
            _innovateTo = ressourceTo;
            return this;
        }

        public Manager Create()
        {
            return new Manager(_name,_bonus,_dividends,_promotion,_optimisation,_innovation,_innovateFrom,_innovateTo);
        }
    }
}