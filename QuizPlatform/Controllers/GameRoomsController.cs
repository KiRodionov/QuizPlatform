using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Models;
using QuizPlatform.Data;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography;
using QuizPlatform.ViewModels;
using System.Net.WebSockets;


namespace QuizPlatform.Controllers
{
    public class GameRoomsController : Controller
    {
        private readonly QuizDbContext _context;

        public GameRoomsController(QuizDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var gameRoom = await _context.GameRooms
                .Include(gr => gr.Quiz)
                .ThenInclude(qz => qz.Questions)
                .ThenInclude(q => q.Answers)
                .SingleOrDefaultAsync(gr => gr.Id == id);

            if (gameRoom == null) return NotFound();

            Player? player = await SearchPlayerInCurRoom(id);
            var gameRoomData = new GameRoomDetailsViewModel(gameRoom, player);

            return View(gameRoomData);
        }
        [HttpPost]
        public async Task<IActionResult> Join(string playerName, int id)
        {
            var gameRoom = await _context.GameRooms
                .SingleOrDefaultAsync(gr => gr.Id == id);
            var cookieKey = "quiz-player-" + id;

            if (gameRoom == null) return NotFound();

            var oldPlayer = await SearchPlayerInCurRoom(id);

            if (oldPlayer != null) return RedirectToAction("Details", new { id });

            if (string.IsNullOrWhiteSpace(playerName)) return BadRequest("Введите имя");

            var token = RandomNumberGenerator.GetHexString(64, false);
            var newPlayer = new Player(playerName, token);

            gameRoom.AddPlayer(newPlayer);
            await _context.SaveChangesAsync();
            
            var cookieOptions = new CookieOptions();
            cookieOptions.Secure = true;
            cookieOptions.HttpOnly = true;
            Response.Cookies.Append(cookieKey, token, cookieOptions);
            return RedirectToAction("Details", new {id});
        }
        [HttpPost]
        public async Task<IActionResult> Answer(int id, int answerId)
        {
            var gameRoom = await _context.GameRooms
                .Include(gr => gr.Quiz)
                .ThenInclude(qz => qz.Questions)
                .ThenInclude(q => q.Answers)
                .Where(gr =>
                    gr.Id == id
                )
                .SingleOrDefaultAsync();

            var quiz = gameRoom?.Quiz;

            if (quiz == null || gameRoom == null) return NotFound();
            var questionIndex = gameRoom.CurrentQuestionIndex;
            if (questionIndex >= quiz.Questions.Count || questionIndex < 0) return BadRequest("Вопрос не найден");

            var question = quiz.Questions[questionIndex];
            var player = await SearchPlayerInCurRoom(id);
            if (player == null) return Unauthorized();

            var answer = question.Answers
                .SingleOrDefault(a => a.Id == answerId);

            if (gameRoom.Status != GameRoom.GameState.InProgress) return BadRequest("квиз закончен или еще не начался");
            if (answer == null) return BadRequest("Недопустимый вариант ответа");

            player.EnterAnswer(question, answer);
            await _context.SaveChangesAsync();

            return Content("Ответ записан");
        }

        private async Task<Player?> SearchPlayerInCurRoom(int roomId)
        {
            var cookieKey ="quiz-player-" + roomId;
            var curToken = Request.Cookies[cookieKey];

            if (string.IsNullOrWhiteSpace(curToken)) return null;

            var player = await _context.Players
                .Include(p => p.Answers)
                .Where(p =>
                    p.AccessToken == curToken &&
                    p.GameRoomId == roomId
                )
                .SingleOrDefaultAsync();

            return player;
        }


    }
}
