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
        }
        public static class AccountController
        {
            private const string ControllerBase = Base + "/AccountController";

            //Account Routes
            public const string GetAccounts = ControllerBase + "/GetAccounts";
        }
    }
}
