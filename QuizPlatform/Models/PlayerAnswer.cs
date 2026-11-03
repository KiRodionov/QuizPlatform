namespace QuizPlatform.Models
{
    public class PlayerAnswer
    {
        public Question Question { get; private set; }
        public Answer Answer { get; private set; }

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
