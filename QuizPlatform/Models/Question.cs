namespace QuizPlatform.Models
{
    public class Question
    {
        public int Id { get; private set; }
        public int QuizId { get; private set; }
        public string Text { get; private set; }
        public IReadOnlyList<Answer> Answers => _answers;
        private List<Answer> _answers;

        public Question(string text)
        {
            Text = text;
            _answers = new List<Answer>();
        }

        public void RemoveAnswer(Answer answer)
        {
            _answers.Remove(answer);
        }

        public void AddAnswer(Answer answer)
        {
            var oldAnswer = _answers.FirstOrDefault(item => item.Text == answer.Text);
            if (oldAnswer is null) _answers.Add(answer);
        }

        public void EditQuestionText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text)) Text = text;
        }

    }
}
