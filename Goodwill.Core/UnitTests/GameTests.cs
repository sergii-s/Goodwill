using System;
using System.Collections.Generic;
using System.Linq;
using Goodwill.Core;
using Goodwill.Core.Events;
using NFluent;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class GameTests
    {
        private static string _p1 = "Player 1";
        private static string _p2 = "Player 2";
        private static string _p3 = "Player 3";
        private static string _p4 = "Player 4";
        private readonly string _athena = Constants.Athena;
        private readonly string _mercury = Constants.Mercury;
        private readonly string _jupiter = Constants.Jupiter;

        [Test]
        public void Default_config_for_companies()
        {
            var game = new Goodwill.Core.Goodwill(new DefaultGameParameters(), new TestGameInitializer());
            var player1 = game.AddPlayer(_p1);
            var player2 = game.AddPlayer(_p2);
            var player3 = game.AddPlayer(_p3);

            game.Start();

            Check.That(game.Companies.Extracting("Name")).ContainsExactly(new DefaultGameParameters().Companies);
            Check.That(game.Companies.Extracting("MarketShare")).ContainsExactly(35, 35, 30);
            Check.That(game.Companies.Extracting("Money")).ContainsExactly(100, 100, 110);
            Check.That(game.Companies.Select(x => x.Manager).Extracting("Name"))
                .ContainsExactly("Helene", "Gaston", "Alphonse");
            Check.That(player1.Money).IsEqualTo(20);
            Check.That(player1.Actions.Count).IsEqualTo(10);
            Check.That(player2.Money).IsEqualTo(20);
            Check.That(player2.Actions.Count).IsEqualTo(10);
            Check.That(player3.Money).IsEqualTo(20);
            Check.That(player3.Actions.Count).IsEqualTo(10);
        }


        [Test]
        public void Default_config_for_players()
        {
            var game = new Goodwill.Core.Goodwill();
            game.AddPlayer(_p1);
            game.AddPlayer(_p2);
            game.AddPlayer(_p3);
            game.AddPlayer(_p4);

            game.Start();


            Check.That(game.Companies.Extracting("MarketShare")).ContainsExactly(35, 35, 30);
            Check.That(game.Companies.Extracting("Money")).ContainsExactly(100, 100, 110);

            foreach (var player in game.Players)
            {
                if (player.Money == 20)
                {
                    Check.That(player.Money).IsEqualTo(20);
                    Check.That(player.Actions.Count).IsEqualTo(8);
                }
                else
                {
                    Check.That(player.Money).IsEqualTo(33);
                    Check.That(player.Actions.Count).IsEqualTo(7);
                }
            }

            Check.That(game.Players.SelectMany(x => x.Actions)).HasSize(30);
        }

        internal class TestGameInitializer : GameInitializer
        {
            private static int _cnt = -1;

            protected override List<Ressource> GenerateRessorceDependencies()
            {
                _cnt++;
                if (_cnt == 0)
                {
                    return new List<Ressource>() { Ressource.Coal, Ressource.Coal, Ressource.Coal };
                }
                return _cnt == 1
                    ? new List<Ressource>() { Ressource.Fuel, Ressource.Fuel, Ressource.Fuel }
                    : new List<Ressource>() { Ressource.Employee, Ressource.Employee, Ressource.Employee };
            }

            public override void InitializeEvents()
            {
                Goodwill.Events = Config.Events.ToDeck();
                Goodwill.Probabilities = Config.Probabilities.ToDeck();
            }

            protected override void IntitializeManagers()
            {
                foreach (var manager in Config.Managers)
                {
                    Goodwill.Managers.Enqueue(manager);
                }
            }
        }

        [Test]
        public void End_of_year()
        {
            var game = new Goodwill.Core.Goodwill(new DefaultGameParameters(), new TestGameInitializer());
            game.AddPlayer(_p1);
            game.AddPlayer(_p2);
            game.AddPlayer(_p3);
            game.AddPlayer(_p4);

            game.Start();

            var gameInfo1 = game.GetGameInfo();

            Check.That(gameInfo1.Ressources.Select(x => x.Value)).ContainsExactly(5, 5, 5);
            Check.That(gameInfo1.CurrentYear).IsEqualTo(1);
            Check.That(gameInfo1.TotalYears).IsEqualTo(6);

            Check.That(gameInfo1.Companies[_athena].Money).IsEqualTo(100);
            Check.That(gameInfo1.Companies[_athena].MarketShare).IsEqualTo(35);
            Check.That(gameInfo1.Companies[_athena].RessourceDependencies.Select(x => x.Ressource))
                .ContainsExactly(Ressource.Coal, Ressource.Coal, Ressource.Coal);

            Check.That(gameInfo1.Companies[_mercury].Money).IsEqualTo(100);
            Check.That(gameInfo1.Companies[_mercury].MarketShare).IsEqualTo(35);
            Check.That(gameInfo1.Companies[_mercury].RessourceDependencies.Select(x => x.Ressource))
                .ContainsExactly(Ressource.Fuel, Ressource.Fuel, Ressource.Fuel);
            
            Check.That(gameInfo1.Companies[_jupiter].Money).IsEqualTo(110);
            Check.That(gameInfo1.Companies[_jupiter].MarketShare).IsEqualTo(30);
            Check.That(gameInfo1.Companies[_jupiter].RessourceDependencies.Select(x=>x.Ressource))
                .ContainsExactly(Ressource.Employee, Ressource.Employee, Ressource.Employee);

            game.FinishYear();
            var gameInfo2 = game.GetGameInfo();
            Check.That(gameInfo2.CurrentYear).IsEqualTo(2);
            Check.That(gameInfo2.TotalYears).IsEqualTo(6);

            Check.That(gameInfo2.Companies[_athena].Money).IsEqualTo(120);
            Check.That(gameInfo2.Companies[_mercury].Money).IsEqualTo(115);
            Check.That(gameInfo2.Companies[_jupiter].Money).IsEqualTo(115);
        }
        
        [TestCase(-5, 5)]
        [TestCase(0, 5)]
        [TestCase(5, 10)]
        [TestCase(10, 15)]
        [TestCase(15, 20)]
        [TestCase(20, 25)]
        [TestCase(25, 25)]
        public void Event_ressource_test(int move, int res)
        {
            var game = GameWithFourPlayers();

            var gameEvent = Build.Event.PriceChange(Ressource.Coal, move).Create();
            gameEvent.Applicate(game);

            Check.That(game.RessourcePrices[Ressource.Coal]).IsEqualTo(res);
        }


        private static Goodwill.Core.Goodwill GameWithFourPlayers()
        {
            var game = new Goodwill.Core.Goodwill(new DefaultGameParameters(), new TestGameInitializer());
            game.AddPlayer(_p1);
            game.AddPlayer(_p2);
            game.AddPlayer(_p3);
            game.AddPlayer(_p4);
            game.Start();
            return game;
        }


        [Test]
        public void Evaluating_wrong_company()
        {
            var game = GameWithFourPlayers();

            var gameInfo = game.GetGameInfo();
            Check.That(gameInfo.State).IsEqualTo(GameState.Pricing.Company(_athena));
            Check.ThatCode(() => game.SetPrice(_p1, _mercury, 100)).Throws<Exception>();
        }

        [Test]
        public void Evaluating_same_price()
        {
            var game = GameWithFourPlayers();

            var gameInfo = game.GetGameInfo();
            Check.That(gameInfo.State).IsEqualTo(GameState.Pricing.Company(_athena));
            game.SetPrice(_p1, _athena, 10);
            game.SetPrice(_p2, _athena, 10);
            game.SetPrice(_p3, _athena, 10);
            game.SetPrice(_p4, _athena, 10);
            game.Next();

            var gameInfo2 = game.GetGameInfo();
            //TODO check no transactions
            Check.That(gameInfo2.State).IsEqualTo(GameState.VotingForManager.Company(_athena));
        }

        [Test]
        public void Evaluating_one_transaction()
        {
            var game = GameWithFourPlayers();

            var gameInfo = game.GetGameInfo();
            game.SetPrice(_p1, _athena, 8);
            game.SetPrice(_p2, _athena, 10);
            game.SetPrice(_p3, _athena, 10);
            game.SetPrice(_p4, _athena, 12);
            game.Next();

            var gameInfo2 = game.GetGameInfo();
            Check.That(gameInfo2.State).IsEqualTo(GameState.Pricing.Company(_athena));

            Check.That(gameInfo2.ActionsCount(_p1, _athena)).IsEqualTo(gameInfo.ActionsCount(_p1, _athena) - 1);
            Check.That(gameInfo2.ActionsCount(_p4, _athena)).IsEqualTo(gameInfo.ActionsCount(_p4, _athena) + 1);
            Check.That(gameInfo2.PlayerMoney(_p1)).IsEqualTo(gameInfo.PlayerMoney(_p1) + 10);
            Check.That(gameInfo2.PlayerMoney(_p4)).IsEqualTo(gameInfo.PlayerMoney(_p4) - 10);
        }

        [Test]
        public void Evaluating_multiple_transaction()
        {
            var game = GameWithFourPlayers();

            var gameInfo = game.GetGameInfo();
            Check.That(gameInfo.State).IsEqualTo(GameState.Pricing.Company(_athena));

            game.SetPrice(_p1, _athena, 8);
            game.SetPrice(_p2, _athena, 8);
            game.SetPrice(_p3, _athena, 12);
            game.SetPrice(_p4, _athena, 12);
            game.Next();

            var gameInfo2 = game.GetGameInfo();

            Check.That(gameInfo2.State).IsEqualTo(GameState.Pricing.Company(_athena));

            Check.That(gameInfo2.ActionsCount(_p1, _athena)).IsEqualTo(gameInfo.ActionsCount(_p1, _athena) - 1);
            Check.That(gameInfo2.ActionsCount(_p2, _athena)).IsEqualTo(gameInfo.ActionsCount(_p2, _athena) - 1);
            Check.That(gameInfo2.ActionsCount(_p3, _athena)).IsEqualTo(gameInfo.ActionsCount(_p3, _athena) + 1);
            Check.That(gameInfo2.ActionsCount(_p4, _athena)).IsEqualTo(gameInfo.ActionsCount(_p4, _athena) + 1);

            Check.That(gameInfo2.PlayerMoney(_p1)).IsEqualTo(gameInfo.PlayerMoney(_p1) + 10);
            Check.That(gameInfo2.PlayerMoney(_p2)).IsEqualTo(gameInfo.PlayerMoney(_p2) + 10);
            Check.That(gameInfo2.PlayerMoney(_p3)).IsEqualTo(gameInfo.PlayerMoney(_p3) - 10);
            Check.That(gameInfo2.PlayerMoney(_p4)).IsEqualTo(gameInfo.PlayerMoney(_p4) - 10);
        }
    }

    public static class GameInfoExtentions
    {
        public static int PlayerMoney(this GameInfo gameInfo, string playerName)
        {
            return gameInfo.PlayersDictionary[playerName].Money;
        }
        public static int ActionsCount(this GameInfo gameInfo, string playerName, string company)
        {
            return gameInfo.PlayersDictionary[playerName].Actions.Count(x => x.Company == company); ;
        }
    }
}
