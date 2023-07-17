using System.Text.Json.Serialization;

namespace backend.Model
{
    public class Game
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        // Ignoriere die GenreId bei der Serialisierung für das Frontend
        [JsonIgnore]
        public int? GenreId { get; set; }
        public string? Genre { get; set; }
        public string? Company { get; set; }
        public float? Price { get; set; }
        public string? ReleaseDate { get; set; }
    }

}
