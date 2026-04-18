using RPSSL.API.Domain.Common;
using RPSSL.API.Domain.Enums;

namespace RPSSL.API.Domain.Entities
{
    public class Game : Entity
    {
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
            return new Game(id, player, playerChoice, computerChoice);
        }

        //public void Play(IGameService gameService)
        //{
        //    var result = gameService.DetermineWinner(PlayerChoice, ComputerChoice);
        //    Result = result;
        //}
    }
}
