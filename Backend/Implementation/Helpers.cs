using backend.Model;
using System;

namespace backend.Implementation
{
    public static class Helpers
    {
        public static string GetGenreString(int? genreId)
        {
            return genreId switch
            {
                1 => "Horror",
                2 => "Abenteuer",
                3 => "Physik",
                4 => "Survival",
                5 => "Simulation",
                6 => "Aufbauspiel",
                7 => "Robert hat Spaß",
                8 => "RPG",
                9 => "MMO",
                10 => "Krieg",
                11 => "Weltraum",
                12 => "Story",
                13 => "Cyberpunk",
                14 => "FPS",
                _ => "Unknown",
            };
        }

        public static int GetGenreId(string? Genre)
        {
            return Genre switch
            {
                "Horror" => 1,
                "Abenteuer" => 2,
                "Physik" => 3,
                "Survival" => 4,
                "Simulation" => 5,
                "Aufbauspiel" => 6,
                "Robert hat Spaß" => 7,
                "RPG" => 8,
                "MMO" => 9,
                "Krieg" => 10,
                "Weltraum" => 11,
                "Story" => 12,
                "Cyberpunk" => 13,
                "FPS" => 14,
                _ => 0,
            };
        }

        public static Game ConvertToBackendGame(Game G)
        {
            Game newG = new Game { 
                Id = G.Id,
                Name = G.Name,
                GenreId = GetGenreId(G.Genre),
                Company = G.Company,
                Price = G.Price,
                ReleaseDate = G.ReleaseDate
            };
            return newG;
        }
    }
}
