using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Data;
using QuizPlatform.Models;
using System.Diagnostics;
using System.Security.Cryptography;

namespace QuizPlatform.Controllers
{
    public class HomeController : Controller
    {
        private readonly QuizDbContext _context;
        
        public HomeController(QuizDbContext context) 
        {
            _context = context;
        }

        

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var quiz = await _context.Quizzes
                                    .Include(quiz => quiz.Questions)
                                    .ThenInclude(question => question.Answers)
                                    .SingleOrDefaultAsync(quiz => quiz.Id == 1);
            if(quiz != null)
            {
                return View(quiz);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(string answerText)
        {
            var quiz = await _context.Quizzes
                        .Include(quiz => quiz.Questions)
                        .ThenInclude(question => question.Answers)
                        .SingleOrDefaultAsync(quiz => quiz.Id == 1);
            var player = await _context.Players
                        .Include(p => p.Answers)
                        .SingleOrDefaultAsync(p => p.Id == 1);

            if (quiz == null || player == null) return NotFound();

            var question = quiz.Questions.FirstOrDefault();
            if (question == null) return Content("Вопроса не найдено");

            var selectedAnswer = question.Answers.FirstOrDefault(a => a.Text == answerText);
            if(selectedAnswer == null) return Content("Ответа не найдено");

            player.EnterAnswer(question, selectedAnswer);
            var playerAnswer = player.FindAnswer(question);

            await _context.SaveChangesAsync();

            if (playerAnswer == null) return Content("Ответ игрока не найден");

            return Content(playerAnswer.Answer.Text);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestQuizInDatabase()
        {
            var testQuiz = CreateTestQuiz();
            _context.Quizzes.Add(testQuiz);
            await _context.SaveChangesAsync();
            return Content("Квиз сохранён");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTestGameRoomInDataBase()
        {
            var quiz = await _context.Quizzes
                .Include(quiz => quiz.Questions)
                .ThenInclude(question => question.Answers)
                .FirstOrDefaultAsync(quiz => quiz.Id == 1);

            if (quiz == null) return NotFound();

            var player = CreatePlayer("John");
            var gameRoom = CreateGameRoom(quiz);

            gameRoom.AddPlayer(player);
            _context.GameRooms.Add(gameRoom);
            await _context.SaveChangesAsync();

            return Content("Комната создана");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Quiz CreateTestQuiz()
        {
            var quiz = new Quiz("Мой первый квиз");
            var quest1 = new Question("Какой формы Земля?");
            quiz.AddQuestion(quest1);
            quest1.AddAnswer(new Answer("Круглая", true));
            quest1.AddAnswer(new Answer("Плоская", false));
            return quiz;
        }

        private GameRoom CreateGameRoom(Quiz quiz)
        {
            return new GameRoom("#1234", quiz);
        }

        private Player CreatePlayer(string name)
        {
            string token = RandomNumberGenerator.GetHexString(64, false);
            return new Player(name, token);
        }
    }
}
