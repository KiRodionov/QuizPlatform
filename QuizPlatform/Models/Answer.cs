namespace QuizPlatform.Models
{
    public class Answer
    {
        public int Id { get; private set; }
        public int QuestionId { get; private set; }
        public string Text { get; private set; }
        public bool IsCorrect { get; private set; }

        public int Votes { get; private set; }
        public Answer(string text, bool isCorrect)
        {
            Text = text;
            IsCorrect = isCorrect;
            Votes = 0;
        }

        public void AddVote() => Votes++;
        public void DelVote() => Votes--;

        public void EditAnswerText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text)) Text = text;
        }

        public void SetCorrect()
        {
            IsCorrect = true;
        }

        public void SetIncorrect()
        {
            IsCorrect = false;
        }
    }
}
