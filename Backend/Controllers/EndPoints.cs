namespace backend.Controllers
{
    public class Endpoints
    {
        private const string Base = "/api";

        public static class GameController
        {
            private const string ControllerBase = Base + "/GameController";

            // Game Routes
            public const string GetGames = ControllerBase + "/Games";
            public const string GetGameById = ControllerBase + "/Game";
            public const string CreateGame = ControllerBase + "/Create";
            public const string DeleteGame = ControllerBase + "/Delete";
            public const string UpdateGame = ControllerBase + "/Update";

        }
    }
}
