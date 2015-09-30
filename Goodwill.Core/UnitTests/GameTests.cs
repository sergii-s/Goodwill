using System.Collections.Generic;
using System.Linq;
using Goodwill.Core;
using Goodwill.Core.Events;
using NFluent;
using Xunit;

namespace UnitTests
{
    public class GameTests
    {
        [Fact]
        public void Default_config_for_companies()
        {
            var game = new Goodwill.Core.Goodwill(new DefaultGameParameters(), new TestGameInitializer());
            var player1 = game.AddPlayer("Player 1");
            var player2 = game.AddPlayer("Player 2");
            var player3 = game.AddPlayer("Player 3");

            game.Start();

            Check.That(game.Companies.Extracting("Name")).ContainsExactly(new DefaultGameParameters().Companies);
            Check.That(game.Companies.Extracting("MarketShare")).ContainsExactly(35, 35, 30);
            Check.That(game.Companies.Extracting("Money")).ContainsExactly(100, 100, 110);
            Check.That(game.Companies.Select(x => x.Manager).Extracting("Name"))
                .ContainsExactly("Francois", "Derec", "Helene");
            Check.That(player1.Money).IsEqualTo(20);
            Check.That(player1.Actions.Count).IsEqualTo(10);
            Check.That(player2.Money).IsEqualTo(20);
            Check.That(player2.Actions.Count).IsEqualTo(10);
            Check.That(player3.Money).IsEqualTo(20);
            Check.That(player3.Actions.Count).IsEqualTo(10);
        }


        [Fact]
        public void Default_config_for_players()
        {
            var game = new Goodwill.Core.Goodwill();
            game.AddPlayer("Player 1");
            game.AddPlayer("Player 2");
            game.AddPlayer("Player 3");
            game.AddPlayer("Player 4");

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

            protected override List<RessourceInfo> GenerateRessorceDependencies()
            {
                _cnt++;
                if (_cnt == 0)
                {
                    return new List<RessourceInfo>() {RessourceInfo.Coal, RessourceInfo.Coal, RessourceInfo.Coal};
                }
                return _cnt == 1
                    ? new List<RessourceInfo>() {RessourceInfo.Fuel, RessourceInfo.Fuel, RessourceInfo.Fuel}
                    : new List<RessourceInfo>() {RessourceInfo.Employee, RessourceInfo.Employee, RessourceInfo.Employee};
            }

            public override void InitializeEvents(Goodwill.Core.Goodwill goodwill, IGameParameters config)
            {
                goodwill.Events = config.Events.Select((x, i) => new GameEvent(i*5, x)).ToDeck();
            }

            protected override void IntitializeManagers()
            {
                foreach (var manager in Config.Managers)
                {
                    Goodwill.Managers.Enqueue(manager);
                }
            }
        }

        [Fact]
        public void End_of_year()
        {
            var game = new Goodwill.Core.Goodwill(new DefaultGameParameters(), new TestGameInitializer());
            game.AddPlayer("Player 1");
            game.AddPlayer("Player 2");
            game.AddPlayer("Player 3");
            game.AddPlayer("Player 4");

            game.Start();

            var gameInfo1 = game.GetGameInfo();

            Check.That(gameInfo1.Ressources.Select(x => x.Value)).ContainsExactly(5, 5, 5);
            Check.That(gameInfo1.CurrentYear).IsEqualTo(1);
            Check.That(gameInfo1.TotalYears).IsEqualTo(6);

            Check.That(gameInfo1.Companies["Athena"].Money).IsEqualTo(100);
            Check.That(gameInfo1.Companies["Athena"].MarketShare).IsEqualTo(35);
            Check.That(gameInfo1.Companies["Athena"].RessourceDependencies)
                .ContainsExactly(RessourceInfo.Coal, RessourceInfo.Coal, RessourceInfo.Coal);

            Check.That(gameInfo1.Companies["Mercury"].Money).IsEqualTo(100);
            Check.That(gameInfo1.Companies["Mercury"].MarketShare).IsEqualTo(35);
            Check.That(gameInfo1.Companies["Mercury"].RessourceDependencies)
                .ContainsExactly(RessourceInfo.Fuel, RessourceInfo.Fuel, RessourceInfo.Fuel);

            Check.That(gameInfo1.Companies["Jupiter"].Money).IsEqualTo(110);
            Check.That(gameInfo1.Companies["Jupiter"].MarketShare).IsEqualTo(30);
            Check.That(gameInfo1.Companies["Jupiter"].RessourceDependencies)
                .ContainsExactly(RessourceInfo.Employee, RessourceInfo.Employee, RessourceInfo.Employee);

            game.FinishYear();
            var gameInfo2 = game.GetGameInfo();
            Check.That(gameInfo2.CurrentYear).IsEqualTo(2);
            Check.That(gameInfo2.TotalYears).IsEqualTo(6);

            Check.That(gameInfo2.Companies["Athena"].Money).IsEqualTo(120);
            Check.That(gameInfo2.Companies["Mercury"].Money).IsEqualTo(115);
            Check.That(gameInfo2.Companies["Jupiter"].Money).IsEqualTo(115);
        }

        [Theory]
        [InlineData(-5, 5)]
        [InlineData(0, 5)]
        [InlineData(5, 10)]
        [InlineData(10, 15)]
        [InlineData(15, 20)]
        [InlineData(20, 25)]
        [InlineData(25, 25)]
        public void Event_ressource_test(int move, int res)
        {
            var game = new Goodwill.Core.Goodwill(new DefaultGameParameters(), new TestGameInitializer());
            game.AddPlayer("Player 1");
            game.AddPlayer("Player 2");
            game.AddPlayer("Player 3");
            game.AddPlayer("Player 4");
            game.Start();

            var gameEvent = GameEventAction.RessourcePriceMove(RessourceInfo.Coal, move);
            gameEvent.Applicate(game);

            Check.That(game.RessourcePrices[RessourceInfo.Coal]).IsEqualTo(res);
        }
    }
}
