namespace QuizPlatform.Models
{
    public class Player
    {
        public int GameRoomId { get; private set; }
        public string Name { get; private set; }
        public string? AccessToken { get; private set;}
        public int Id { get; private set; }
        private List<PlayerAnswer> _answers;
        public Player(string name, string accessToken)
        {
            Name = name;
            AccessToken = accessToken;
            _answers = new List<PlayerAnswer>();
        }
        public IReadOnlyList<PlayerAnswer> Answers => _answers;
        public IReadOnlyList<PlayerAnswer> GetAnswers() => _answers;
        public PlayerAnswer? FindAnswer(Question question)
        {
            var playerAnswer = _answers.FirstOrDefault(item => item?.Question == question, null);
            return playerAnswer;
        }

        public void EnterAnswer(Question question, Answer answer)
        {
            var playerAnswer = FindAnswer(question);
            if (playerAnswer is not null)
            {
                playerAnswer.ChangeAnswer(answer);
            }
            else
            {
                _answers.Add(new PlayerAnswer(question, answer));
            }
        }

        
        public void ChangeName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
        }

    }
}
