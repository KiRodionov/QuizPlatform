namespace QuizPlatform.Models
{
    public class Player
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        private List<PlayerAnswer> _answers;
        public Player(string name, int id) //ID Нужно создавать при заходе на сайт, пока оставил тут как заглушку
        {
            Name = name;
            ID = id;
            _answers = new List<PlayerAnswer>();
        }
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

        public IReadOnlyList<PlayerAnswer> GetAnswers() => _answers;
        public void ChangeName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name)) Name = name;
        }

    }
}
