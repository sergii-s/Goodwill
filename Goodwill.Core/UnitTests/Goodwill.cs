using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class Goodwill
    {
        private readonly List<Company> _companies = new List<Company>();
        private readonly IGameParameters _config;
        private readonly List<Player> _players = new List<Player>();

        public Goodwill()
            : this(new DefaultGameParameters())
        {
        }

        public Goodwill(IGameParameters config)
        {
            _config = config;
        }

        public List<Company> Companies
        {
            get { return _companies; }
        }

        public List<Player> Players
        {
            get { return _players; }
        }

        public Player AddPlayer(string playerName)
        {
            var newPlayer = new Player
            {
                Name = playerName
            };
            _players.Add(newPlayer);

            return newPlayer;
        }

        public void Start()
        {
            InitializeCompanies();
            InitializeMarketPart();
            InitializePlayers();
            DistributeActions();
        }

        private void DistributeActions()
        {
            var totalActions = _config.ActionsByCompany * _config.Companies.Length;
            var actionsByPlayer = totalActions / _players.Count;
            var actionsLeft = totalActions - actionsByPlayer * _players.Count;

            var allActions = _companies.SelectMany(x => x.Actions).Shuffle().ToList();

            foreach (Player player in _players)
            {
                player.Actions = allActions.Take(actionsByPlayer).ToList();
            }
        }

        private void InitializePlayers()
        {
            foreach (Player player in _players)
            {
                player.Money = _config.InitialPlayerMoney;
            }
        }

        private void InitializeCompanies()
        {
            foreach (string companyName in _config.Companies)
            {
                var company = new Company
                {
                    Name = companyName,
                    Money = _config.InitialCompanyMoney,
                    Actions = new List<CompanyAction>()
                };
                for (int i = 0; i < _config.ActionsByCompany; i++)
                {
                    company.Actions.Add(new CompanyAction
                    {
                        Company = company
                    });
                }
                _companies.Add(company);
            }
        }

        private void InitializeMarketPart()
        {
            const int total = 100;
            var parts = total / _config.MarketPartDivider;
            var partsByCompany = parts / Companies.Count;
            var partsLeft = parts - (partsByCompany * Companies.Count);

            var ctr = 0;
            foreach (Company company in Companies)
            {
                company.MarketPart = _config.MarketPartDivider * partsByCompany;
                if (partsLeft != 0)
                {
                    if (ctr < partsLeft)
                    {
                        company.MarketPart += _config.MarketPartDivider;
                    }
                    else
                    {
                        company.Money += _config.BonusCompanyMoneyPerMarketPart;
                    }
                }
                ctr++;
            }
        }
    }

    public static class Extentions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items)
        {
            var rnd = new Random((int) DateTime.Now.Ticks);
            return items.OrderBy(item => rnd.Next());
        }
    }
}