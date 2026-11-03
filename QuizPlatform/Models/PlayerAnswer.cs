namespace QuizPlatform.Models
{
    public class PlayerAnswer
    {
        public int Id { get; private set; }
        public int QuestionId { get; private set; }
        public int AnswerId { get; private set; }
        public int PlayerId { get; private set; }
        public Question Question { get; private set; } = null!;
        public Answer Answer { get; private set; } = null!;

        private PlayerAnswer() { }
        public PlayerAnswer(Question question, Answer answer)
        {
            Question = question;
            Answer = answer;
            Answer.AddVote();
        }

        public void ChangeAnswer(Answer answer)
        {
            Answer.DelVote();
            Answer = answer;
            Answer.AddVote();
        }

    }
}
