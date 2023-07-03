using Microsoft.AspNetCore.Mvc;
using backend.Model;
using backend.DataProvider;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        private readonly ILogger<GameController> _logger;
        private readonly SqlDataProvider _dataProvider;

        public GameController(ILogger<GameController> logger, SqlDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        // default returns entire DB
        [HttpGet(Endpoints.GameController.GetGames, Name = "GetGames")]
        public IEnumerable<Game> GetGames(int amount = 0)
        {
            if(amount > 0)
            {
                return _dataProvider.Games.Take(amount).ToList();
            }
            return _dataProvider.Games.ToList();
        }

        [HttpPost("Games")]
        public IActionResult CreateGame([FromBody] Game game)
        {
            if (game == null)
            {
                return BadRequest(); // Ung�ltige Anforderung
            }

            _dataProvider.Games.Add(game); // Spiel der Datenquelle hinzuf�gen
            _dataProvider.SaveChanges(); // Speichere die �nderungen in der Datenbank

            return CreatedAtRoute("GetGameById", new { id = game.Id }, game); // Erfolgreiches Erstellen mit 201 Created-Status und dem erstellten Spiel als Antwort
        }


        [HttpDelete("Games/{id}")]
        public IActionResult DeleteGame(int id)
        {
            var game = _dataProvider.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound(); // Falls das Spiel mit der angegebenen ID nicht gefunden wurde
            }

            _dataProvider.Games.Remove(game); // Spiel aus der Datenquelle entfernen
            _dataProvider.SaveChanges(); // Speichere die �nderungen in der Datenbank

            return NoContent(); // Erfolgreiches L�schen (kein Inhalt zur�ckgegeben)
        }
    }
}