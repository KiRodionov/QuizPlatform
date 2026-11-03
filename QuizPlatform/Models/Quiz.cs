using Microsoft.EntityFrameworkCore;

namespace QuizPlatform.Models
{
    public class Quiz
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        private List<Question> _questions;
        public IReadOnlyList<Question> Questions => _questions;
        public Quiz(string title)
        {
            Title = title;
            _questions = new List<Question>();
        }

        public void EditTitle(string title)
        {
            if (!string.IsNullOrWhiteSpace(title)) Title = title;
        }
       
        public void AddQuestion(Question question)
        {
            var oldQuestion = _questions.FirstOrDefault(item => item.Text == question.Text);
            if (oldQuestion is null) _questions.Add(question);
        }

        public void RemoveQuestion(Question question)
        {
            _questions.Remove(question);
        }


    }
}
