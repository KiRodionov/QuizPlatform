namespace QuizPlatform.Models
{
    public class GameRoom
    {
        public enum GameState
        {
            Waiting,
            InProgress,
            Finished
        }
        public int Id { get; private set; }
        public int QuizId { get; private set; }
        public int CurrentQuestionIndex { get; private set; } = 0;
        public string Code { get; private set; } = null!;
        public Quiz Quiz { get; private set; } = null!;
        public GameState Status { get; private set; }
        private List<Player> _players = new List<Player>();
        private GameRoom() { }
        public GameRoom(string code, Quiz quiz)
        {
            Code = code;
            Quiz = quiz;
            Status = GameState.Waiting;
        }
        public IReadOnlyList<Player> Players => _players;

        public IReadOnlyList<Player> GetPlayers() => _players;

        public void AddPlayer(Player player) => _players.Add(player);

        public void StartGame()
        {
            if (Status == GameState.Waiting && _players.Count != 0 && Quiz.Questions.Count != 0) Status = GameState.InProgress;
        }

        public void NextQuestion()
        {
            if (Status != GameState.InProgress) return;
            if (CurrentQuestionIndex < Quiz.Questions.Count - 1)
            {
                CurrentQuestionIndex++;
            }
            else
            {
                Status = GameState.Finished;
            }
        }

        public void ChangePlayerAnswer(Player player, Question question, Answer newAnswer)
        {
            player.EnterAnswer(question, newAnswer);
        }

    }
}
