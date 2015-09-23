using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    class GameInitializer : IGameInitializer
    {
        private IGameParameters _config;
        private Goodwill _goodwill;

        public void InitializeGame(Goodwill goodwill, IGameParameters config)
        {
            _config = config;
            _goodwill = goodwill;
            InitializeCompanies();
            InitializeMarketPart();
            InitializePlayers();
            DistributeActions();
        }

        private void DistributeActions()
        {
            var totalActions = _config.ActionsByCompany * _config.Companies.Length;
            var actionsByPlayer = totalActions / _goodwill.Players.Count;
            var actionsLeft = totalActions - actionsByPlayer * _goodwill.Players.Count;

            var allActions = _goodwill.Companies.SelectMany(x => x.Actions).Shuffle();

            foreach (var player in _goodwill.Players)
            {
                player.Actions = allActions.Pick(actionsByPlayer);
            }

            if (actionsLeft == 0) return;

            foreach (var player in _goodwill.Players.Shuffle())
            {
                if (actionsLeft != 0)
                {
                    player.Actions.Add(allActions.Pick());
                    actionsLeft--;
                }
                else
                {
                    player.Money += _config.BonusPlayerMoneyPerMissingAction;
                }
            }
        }

        private void InitializePlayers()
        {
            foreach (Player player in _goodwill.Players)
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
                for (var i = 0; i < _config.ActionsByCompany; i++)
                {
                    company.Actions.Add(new CompanyAction
                    {
                        Company = company
                    });
                }
                _goodwill.Companies.Add(company);
            }
        }

        private void InitializeMarketPart()
        {
            const int total = 100;
            var parts = total / _config.MarketPartDivider;
            var partsByCompany = parts / _goodwill.Companies.Count;
            var partsLeft = parts - (partsByCompany * _goodwill.Companies.Count);

            var ctr = 0;
            foreach (Company company in _goodwill.Companies)
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
}