using backend.DataProvider;
using backend.Implementation;
using backend.Model;
using Microsoft.AspNetCore.Mvc;

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

        // by default returns 54 entries, if amount == 0, return all
        //      this will introduce a bug when you filter for exactly 54 games (amount == 54)
        //      in this case the amount filter will not work and it returns all entries
        [HttpGet(Endpoints.GameController.GetGames, Name = "GetGames")]
        public IEnumerable<Game> GetGames(
            int amount = 54,
            string filter = "",
            string gFilter = "",
            float priceFilter = -1,
            string companyFilter = "",
            string minRDFilter = "",
            string maxRDFilter = "")
        {
            var games = _dataProvider.Games.AsQueryable();

            bool f = false;
            int amountBackup = 0;
            if (amount != 54)
            {
                f = true;
                amountBackup = amount;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                games = games.Where(g => g.Name.Contains(filter));
                amount = 0; 
            }

            if (!string.IsNullOrEmpty(gFilter))
            {
                games = games.Where(g => g.GenreId == Helpers.GetGenreId(gFilter));
                amount = 0;
            }

            if (priceFilter != -1)
            {
                games = games.Where(g => g.Price == priceFilter);
                amount = 0;
            }

            if (!string.IsNullOrEmpty(companyFilter))
            {
                games = games.Where(g => g.Company.Contains(companyFilter));
                amount = 0;
            }

            if (!string.IsNullOrEmpty(minRDFilter))
            {
                games = games.Where(g => string.Compare(g.ReleaseDate, minRDFilter) >= 0);
                amount = 0;
            }

            if (!string.IsNullOrEmpty(maxRDFilter))
            {
                games = games.Where(g => string.Compare(g.ReleaseDate, maxRDFilter) <= 0);
                amount = 0;
            }
            if(f)
            {
                games = games.Take(amountBackup);
            }
            if (amount > 0 && !f)
            {
                games = games.Take(amount);
            }

            //Hier wird die Genre-Id in einen Genre-String umgewandelt
           games = games.Select(g => new Game
           {
               Id = g.Id,
               Name = g.Name,
               Genre = Helpers.GetGenreString(g.GenreId),
               Company = g.Company,
               Price = g.Price,
               ReleaseDate = g.ReleaseDate
           });

            return games.ToList();
        }


        [HttpPost("Games", Name = "CreateGame")]
        public IActionResult CreateGame([FromBody] Game game)
        {
            if (game == null)
            {
                return BadRequest(); // Ungültige Anforderung
            }

            _dataProvider.Games.Add(Helpers.ConvertToBackendGame(game)); // Spiel der Datenquelle hinzufügen
            _dataProvider.SaveChanges(); // Speichere die Änderungen in der Datenbank

            return CreatedAtRoute("CreateGame", new { id = game.Id }, game); // Erfolgreiches Erstellen mit 201 Created-Status und dem erstellten Spiel als Antwort
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
            _dataProvider.SaveChanges(); // Speichere die Änderungen in der Datenbank

            return NoContent(); // Erfolgreiches Löschen (kein Inhalt zurückgegeben)
        }

        [HttpPut("Games/{id}")]
        public IActionResult UpdateGame(int id, [FromBody] Game updatedGame)
        {
            var game = _dataProvider.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound(); // Falls das Spiel mit der angegebenen ID nicht gefunden wurde
            }

            // Aktualisiere die Eigenschaften des Spiels mit den Werten aus updatedGame
            game.Name = updatedGame.Name;
            game.GenreId = Helpers.GetGenreId(updatedGame.Genre);
            game.Company = updatedGame.Company;
            game.Price = updatedGame.Price;
            game.ReleaseDate = updatedGame.ReleaseDate;

            // Speichere die Änderungen in der Datenbank
            _dataProvider.SaveChanges();

            return Ok(); // Erfolgreiche Aktualisierung
        }

    }
}