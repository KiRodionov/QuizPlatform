using Microsoft.EntityFrameworkCore;
using QuizPlatform.Models;

namespace QuizPlatform.Data 
{
    public class QuizDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes{ get; set; }
        public DbSet<Answer> Answers{ get; set; }
        public DbSet<Question> Questions { get; set; }
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quiz>()
                .HasMany(quiz => quiz.Questions)
                .WithOne()
                .HasForeignKey(question => question.QuizId)
                .IsRequired();

            modelBuilder.Entity<Question>()
                .HasMany(question => question.Answers)
                .WithOne()
                .HasForeignKey(answer => answer.QuestionId)
                .IsRequired();
        }
    }
}
