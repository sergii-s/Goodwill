using NFluent;
using Xunit;

namespace UnitTests
{
    public class GameTests
    {
        [Fact]
        public void DefaultConfigTest()
        {
            var defaultGameParameters = new DefaultGameParameters();
            var game = new Goodwill(defaultGameParameters);
            var player1 = game.AddPlayer("Player 1");
            var player2 = game.AddPlayer("Player 2");
            var player3 = game.AddPlayer("Player 3");

            game.Start();

            var totalActions = defaultGameParameters.ActionsByCompany*defaultGameParameters.Companies.Length;

            Check.That(game.Companies.Extracting("Name")).ContainsExactly(defaultGameParameters.Companies);
            Check.That(game.Companies.Extracting("MarketPart")).ContainsExactly(new[] { 35, 35, 30 });
            Check.That(game.Companies.Extracting("Money")).ContainsExactly(new[] { 100, 100, 110 });
            Check.That(player1.Money).IsEqualTo(20);
            Check.That(player1.Actions.Count).IsEqualTo(10);
            Check.That(player2.Money).IsEqualTo(20);
            Check.That(player2.Actions.Count).IsEqualTo(10);
            Check.That(player3.Money).IsEqualTo(20);
            Check.That(player3.Actions.Count).IsEqualTo(10);
        }


        [Fact]
        public void DefaultConfig_four_players_Test()
        {
            var defaultGameParameters = new DefaultGameParameters();
            var game = new Goodwill(defaultGameParameters);
            var player1 = game.AddPlayer("Player 1");
            var player2 = game.AddPlayer("Player 2");
            var player3 = game.AddPlayer("Player 3");
            var player4 = game.AddPlayer("Player 4");

            game.Start();

            var totalActions = defaultGameParameters.ActionsByCompany * defaultGameParameters.Companies.Length;

            Check.That(game.Companies.Extracting("Name")).ContainsExactly(defaultGameParameters.Companies);
            Check.That(game.Companies.Extracting("MarketPart")).ContainsExactly(new[] { 35, 35, 30 });
            Check.That(game.Companies.Extracting("Money")).ContainsExactly(new[] { 100, 100, 110 });
            Check.That(player1.Money).IsEqualTo(20);
            Check.That(player1.Actions.Count).IsEqualTo(8);
            Check.That(player2.Money).IsEqualTo(20);
            Check.That(player2.Actions.Count).IsEqualTo(8);
            Check.That(player3.Money).IsEqualTo(20);
            Check.That(player3.Actions.Count).IsEqualTo(7);
            Check.That(player4.Money).IsEqualTo(33);
            Check.That(player4.Actions.Count).IsEqualTo(7);
        }
    }
}
