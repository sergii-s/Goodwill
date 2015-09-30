using System;
using System.Collections.Generic;
using System.Linq;
using Goodwill.Core.Events;

namespace Goodwill.Core
{
    public class GameInitializer : IGameInitializer
    {
        protected IGameParameters Config;
        protected Goodwill Goodwill;

        public void InitializeGame(Goodwill goodwill, IGameParameters config)
        {
            Config = config;
            Goodwill = goodwill;
            IntitializeManagers();
            InitializeCompanies();
            InitializeRessources();
            InitializeMarketPart();
            InitializePlayers();
            DistributeActions();
        }

        public virtual void InitializeEvents(Goodwill goodwill, IGameParameters config)
        {
            goodwill.Events = config.Events.Select(x => new GameEvent(5, x)).Shuffle();
        }

        protected virtual void IntitializeManagers()
        {
            foreach (var manager in Config.Managers.Shuffle())
            {
                Goodwill.Managers.Enqueue(manager);
            }
        }

        private void InitializeRessources()
        {
            foreach (var ressource in Config.Ressources)
            {
                Goodwill.RessourcePrices[ressource] = Config.InitialRessourcePrice;
            }
        }

        private void DistributeActions()
        {
            var totalActions = Config.ActionsByCompany * Config.Companies.Length;
            var actionsByPlayer = totalActions / Goodwill.Players.Count;
            var actionsLeft = totalActions - actionsByPlayer * Goodwill.Players.Count;

            var allActions = Goodwill.Companies.SelectMany(x => x.Actions).Shuffle();

            foreach (var player in Goodwill.Players)
            {
                player.Actions = allActions.Pick(actionsByPlayer);
            }

            if (actionsLeft == 0) return;

            foreach (var player in Goodwill.Players.Shuffle())
            {
                if (actionsLeft != 0)
                {
                    player.Actions.Add(allActions.Pick());
                    actionsLeft--;
                }
                else
                {
                    player.Money += Config.BonusPlayerMoneyPerMissingAction;
                }
            }
        }

        private void InitializePlayers()
        {
            foreach (Player player in Goodwill.Players)
            {
                player.Money = Config.InitialPlayerMoney;
            }
        }

        private void InitializeCompanies()
        {
            foreach (string companyName in Config.Companies)
            {
                var company = new Company
                {
                    Name = companyName,
                    Money = Config.InitialCompanyMoney,
                    RessourceDependencies = GenerateRessorceDependencies(),
                    Actions = new List<CompanyAction>(),
                    Manager = Goodwill.Managers.Pick()
                };
                for (var i = 0; i < Config.ActionsByCompany; i++)
                {
                    company.Actions.Add(new CompanyAction
                    {
                        Company = company
                    });
                }
                Goodwill.Companies.Add(company);
            }
        }

        protected virtual List<RessourceInfo> GenerateRessorceDependencies()
        {
            var ressourceInfos = Enum.GetValues(typeof (RessourceInfo)).Cast<RessourceInfo>().ToList();
            return ressourceInfos.GenerateRandom(3).ToList();
        }

        private void InitializeMarketPart()
        {
            const int total = 100;
            var parts = total / Config.MarketPartDivider;
            var partsByCompany = parts / Goodwill.Companies.Count;
            var partsLeft = parts - (partsByCompany * Goodwill.Companies.Count);

            var ctr = 0;
            foreach (Company company in Goodwill.Companies)
            {
                company.MarketShare = Config.MarketPartDivider * partsByCompany;
                if (partsLeft != 0)
                {
                    if (ctr < partsLeft)
                    {
                        company.MarketShare += Config.MarketPartDivider;
                    }
                    else
                    {
                        company.Money += Config.BonusCompanyMoneyPerMarketPart;
                    }
                }
                ctr++;
            }
        }
    }
}