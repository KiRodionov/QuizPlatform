using QuizPlatform.Models;

namespace QuizPlatform.ViewModels
{
    public class GameRoomDetailsViewModel
    {
       public GameRoom GameRoom { get; private set; }
        public Player? CurrentPlayer { get; private set; }

       public GameRoomDetailsViewModel(GameRoom gameRoom, Player? player = null)
       {
            GameRoom = gameRoom;
            CurrentPlayer = player;
       }
    }
}
