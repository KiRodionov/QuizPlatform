using Microsoft.EntityFrameworkCore;
using QuizPlatform.Models;

namespace QuizPlatform.Data 
{
    public class QuizDbContext : DbContext
    {
        public DbSet<Quiz> Quizzes{ get; set; }
        public DbSet<Answer> Answers{ get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<GameRoom> GameRooms { get; set; }
        public DbSet<PlayerAnswer> PlayerAnswers { get; set; }
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

            modelBuilder.Entity<GameRoom>()
                .HasMany(gameRoom => gameRoom.Players)
                .WithOne()
                .HasForeignKey(player => player.GameRoomId)
                .IsRequired();

            modelBuilder.Entity<GameRoom>()
                .HasOne(gameRoom => gameRoom.Quiz)
                .WithMany()
                .HasForeignKey(gameRoom => gameRoom.QuizId)
                .IsRequired();

            modelBuilder.Entity<Player>()
                .HasMany(player => player.Answers)
                .WithOne()
                .HasForeignKey(playerAnswer => playerAnswer.PlayerId)
                .IsRequired();

            modelBuilder.Entity<PlayerAnswer>()
                .HasOne(playerAnswer => playerAnswer.Question)
                .WithMany()
                .HasForeignKey(playerAnswer => playerAnswer.QuestionId)
                .IsRequired();

            modelBuilder.Entity<PlayerAnswer>()
                .HasOne(playerAnswer => playerAnswer.Answer)
                .WithMany()
                .HasForeignKey(playerAnswer => playerAnswer.AnswerId)
                .IsRequired();

            modelBuilder.Entity<PlayerAnswer>()
               .HasIndex(pa => new { pa.PlayerId, pa.QuestionId })
               .IsUnique();
        }
    }
}
