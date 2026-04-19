using RPSSL.API.Domain.Common;
using RPSSL.API.Domain.Enums;

namespace RPSSL.API.Domain.Entities
{
    public class Game : Entity
    {
        private static readonly Dictionary<Choice, Choice[]> WinningMoves = new()
        {
            [Choice.Rock]     = [Choice.Scissors, Choice.Lizard],
            [Choice.Paper]    = [Choice.Rock,     Choice.Spock],
            [Choice.Scissors] = [Choice.Paper,    Choice.Lizard],
            [Choice.Lizard]   = [Choice.Paper,    Choice.Spock],
            [Choice.Spock]    = [Choice.Rock,     Choice.Scissors]
        };

        public Choice PlayerChoice { get; private set; }
        public Choice ComputerChoice { get; private set; }
        public GameResult Result { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Player Player { get; private set; }

        private Game(Guid id, Player player, Choice playerChoice, Choice computerChoice) : base(id)
        {
            Player = player;
            PlayerChoice = playerChoice;
            ComputerChoice = computerChoice;
            CreatedAt = DateTime.UtcNow;
        }

        public static Game Create(Guid id, Player player, Choice playerChoice, Choice computerChoice)
        {
            var game = new Game(id, player, playerChoice, computerChoice);
            game.Play();
            return game;
        }

        private void Play()
        {
            if (PlayerChoice == ComputerChoice)
            {
                Result = GameResult.Tie;
                return;
            }

            Result = WinningMoves[PlayerChoice].Contains(ComputerChoice)
                ? GameResult.Win
                : GameResult.Lose;
        }
    }
}
