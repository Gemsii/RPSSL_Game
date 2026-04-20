using RPSSL.API.Domain.Entities;
using RPSSL.API.Domain.Enums;
using RPSSL.API.Domain.ValueObjects;

namespace RPSSL.Tests.Domain
{
    public class GameEntityTests
    {
        private static readonly Player TestPlayer =
            Player.Create(Guid.NewGuid(), PlayerName.Create("TestPlayer"));

        // Win scenarios

        [Theory]
        [InlineData(Choice.Rock,     Choice.Scissors, "Rock crushes Scissors")]
        [InlineData(Choice.Rock,     Choice.Lizard,   "Rock crushes Lizard")]
        [InlineData(Choice.Paper,    Choice.Rock,     "Paper covers Rock")]
        [InlineData(Choice.Paper,    Choice.Spock,    "Paper disproves Spock")]
        [InlineData(Choice.Scissors, Choice.Paper,    "Scissors cuts Paper")]
        [InlineData(Choice.Scissors, Choice.Lizard,   "Scissors decapitates Lizard")]
        [InlineData(Choice.Lizard,   Choice.Paper,    "Lizard eats Paper")]
        [InlineData(Choice.Lizard,   Choice.Spock,    "Lizard poisons Spock")]
        [InlineData(Choice.Spock,    Choice.Rock,     "Spock vaporizes Rock")]
        [InlineData(Choice.Spock,    Choice.Scissors, "Spock smashes Scissors")]
        public void Game_Result_IsWin_ForAllWinningCombinations(Choice player, Choice computer, string scenario)
        {
            var game = Game.Create(Guid.NewGuid(), TestPlayer, player, computer);

            Assert.True(game.Result == GameResult.Win, $"Expected Win: {scenario}");
        }

        // Lose scenarios

        [Theory]
        [InlineData(Choice.Scissors, Choice.Rock,     "Rock crushes Scissors")]
        [InlineData(Choice.Lizard,   Choice.Rock,     "Rock crushes Lizard")]
        [InlineData(Choice.Rock,     Choice.Paper,    "Paper covers Rock")]
        [InlineData(Choice.Spock,    Choice.Paper,    "Paper disproves Spock")]
        [InlineData(Choice.Paper,    Choice.Scissors, "Scissors cuts Paper")]
        [InlineData(Choice.Lizard,   Choice.Scissors, "Scissors decapitates Lizard")]
        [InlineData(Choice.Paper,    Choice.Lizard,   "Lizard eats Paper")]
        [InlineData(Choice.Spock,    Choice.Lizard,   "Lizard poisons Spock")]
        [InlineData(Choice.Rock,     Choice.Spock,    "Spock vaporizes Rock")]
        [InlineData(Choice.Scissors, Choice.Spock,    "Spock smashes Scissors")]
        public void Game_Result_IsLose_WhenComputerWins(Choice player, Choice computer, string scenario)
        {
            var game = Game.Create(Guid.NewGuid(), TestPlayer, player, computer);

            Assert.True(game.Result == GameResult.Lose, $"Expected Lose: {scenario}");
        }

        // Tie scenarios

        [Theory]
        [InlineData(Choice.Rock)]
        [InlineData(Choice.Paper)]
        [InlineData(Choice.Scissors)]
        [InlineData(Choice.Lizard)]
        [InlineData(Choice.Spock)]
        public void Game_Result_IsTie_WhenBothChoicesAreIdentical(Choice choice)
        {
            var game = Game.Create(Guid.NewGuid(), TestPlayer, choice, choice);

            Assert.Equal(GameResult.Tie, game.Result);
        }

        // Win/Lose symmetry
        // If the player wins with X vs Y, he must lose with Y vs X

        [Theory]
        [InlineData(Choice.Rock,     Choice.Scissors)]
        [InlineData(Choice.Rock,     Choice.Lizard)]
        [InlineData(Choice.Paper,    Choice.Rock)]
        [InlineData(Choice.Paper,    Choice.Spock)]
        [InlineData(Choice.Scissors, Choice.Paper)]
        [InlineData(Choice.Scissors, Choice.Lizard)]
        [InlineData(Choice.Lizard,   Choice.Paper)]
        [InlineData(Choice.Lizard,   Choice.Spock)]
        [InlineData(Choice.Spock,    Choice.Rock)]
        [InlineData(Choice.Spock,    Choice.Scissors)]
        public void Game_Result_IsLose_WhenChoicesAreSwapped(Choice winning, Choice losing)
        {
            var win  = Game.Create(Guid.NewGuid(), TestPlayer, winning, losing);
            var lose = Game.Create(Guid.NewGuid(), TestPlayer, losing,  winning);

            Assert.Equal(GameResult.Win,  win.Result);
            Assert.Equal(GameResult.Lose, lose.Result);
        }
    }
}
