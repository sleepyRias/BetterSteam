using backend.DataProvider;
using backend.Implementation;
using backend.Model;
using backend.Model.DTO;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {

        private readonly ILogger<GameController> _logger;
        private readonly GameDataProvider _dataProvider;

        public GameController(ILogger<GameController> logger, GameDataProvider dataProvider)
        {
            _logger = logger;
            _dataProvider = dataProvider;
        }

        [HttpGet(Endpoints.GameController.GetGameById, Name = "GetGameById")]
        public IActionResult GetGames(int id)
        {
            var game = _dataProvider.Games.FirstOrDefault(g => g.Id == id);
            if (game == null)
            {
                return NotFound(); // Falls das Spiel mit der angegebenen ID nicht gefunden wurde
            }

            return Ok(game);
        }


        [HttpGet(Endpoints.GameController.GetGames, Name = "GetGames")]
        public GetGamesResult GetGames(
            int page = 1,
            int pageSize = 20,
            string filter = "",
            string gFilter = "",
            float priceMinFilter = -1.0f,
            float priceMaxFilter = -1.0f,
            string companyFilter = "",
            string minRDFilter = "",
            string maxRDFilter = "")
        {
            var games = _dataProvider.Games.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                games = games.Where(g => g.Name.Contains(filter));
            }

            if (!string.IsNullOrEmpty(gFilter))
            {
                games = games.Where(g => g.GenreId == Helpers.GetGenreId(gFilter));
            }

            if (priceMinFilter != -1)
            {
                games = games.Where(g => g.Price >= priceMinFilter);
            }
            if (priceMaxFilter != -1)
            {
                games = games.Where(g => g.Price <= priceMaxFilter);
            }

            if (!string.IsNullOrEmpty(companyFilter))
            {
                games = games.Where(g => g.Company.Contains(companyFilter));
            }

            if (!string.IsNullOrEmpty(minRDFilter))
            {
                games = games.Where(g => string.Compare(g.ReleaseDate, minRDFilter) >= 0);
            }

            if (!string.IsNullOrEmpty(maxRDFilter))
            {
                games = games.Where(g => string.Compare(g.ReleaseDate, maxRDFilter) <= 0);
            }

            int totalItems = games.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1)
            {
                page = 1;
            }
            else if (page > totalPages)
            {
                page = totalPages;
            }

            games = games.Skip((page - 1) * pageSize).Take(pageSize);

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

            var result = new GetGamesResult
            {
                Games = games.ToList(),
                TotalCount = totalItems
            };

            return result;
        }


        [HttpPost(Endpoints.GameController.CreateGame, Name = "CreateGame")]
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


        [HttpDelete(Endpoints.GameController.DeleteGame)]
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

        [HttpPut(Endpoints.GameController.UpdateGame)]
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

        [HttpGet(Endpoints.GameController.GetGamesAutocomplete, Name = "GetGamesAutocomplete")]
        public GetGamesResult GetGamesAutocomplete(string filter = "")
        {
            var batchSize = 5;
            var games = _dataProvider.Games.AsQueryable();

            if (!string.IsNullOrEmpty(filter))
            {
                games = games.Where(g => g.Name.StartsWith(filter)).OrderBy(g => g.Name);
                games = games.Take(batchSize);
            }

            int totalItems = games.Count();

            // Hier wird die Genre-Id in einen Genre-String umgewandelt
            games = games.Select(g => new Game
            {
                Id = g.Id,
                Name = g.Name,
                Genre = Helpers.GetGenreString(g.GenreId),
                Company = g.Company,
                Price = g.Price,
                ReleaseDate = g.ReleaseDate
            });

            var result = new GetGamesResult
            {
                Games = games.ToList(),
                TotalCount = totalItems
            };

            return result;
        }
    }
}