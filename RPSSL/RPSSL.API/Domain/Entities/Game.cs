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

        private Game(Guid id, Player player, Choice playerChoice, Choice computerChoice, DateTime? createdAt = null) : base(id)
        {
            Player = player;
            PlayerChoice = playerChoice;
            ComputerChoice = computerChoice;
            CreatedAt = createdAt ?? DateTime.UtcNow;
        }

        /// <summary>
        /// Creates a new game and immediately determines its result.
        /// Optionally accepts a <paramref name="createdAt"/> timestamp for restoring persisted games.
        /// </summary>
        public static Game Create(Guid id, Player player, Choice playerChoice, Choice computerChoice, DateTime? createdAt = null)
        {
            var game = new Game(id, player, playerChoice, computerChoice, createdAt ?? DateTime.UtcNow);
            game.Play();
            return game;
        }

        /// <summary>
        /// Determines the game result based on the player and computer choices.
        /// </summary>
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
