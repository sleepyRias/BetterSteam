namespace backend.Controllers
{
    public class Endpoints
    {
        private const string Base = "/api";

        public static class GameController
        {
            private const string ControllerBase = Base + "/Game";

            // Game Routes
            public const string GetGames = ControllerBase + "/Games";
            public const string GetGameById = ControllerBase + "/GameById";
            public const string CreateGame = ControllerBase + "/Create";
            public const string DeleteGame = ControllerBase + "/Delete";
            public const string UpdateGame = ControllerBase + "/Update";
            public const string GameIdsRequest = ControllerBase + "/GamesById";
            public const string GetGamesAutocomplete = ControllerBase + "/GetGamesAutocomplete";
        }
        public static class AccountController
        {
            private const string ControllerBase = Base + "/Account";

            //Account Routes
            public const string GetAccounts = ControllerBase + "/GetAccounts";
            public const string GetAccountById = ControllerBase + "/GetAccountById";
            public const string Login = ControllerBase + "/Login";
            public const string CreateAccount = ControllerBase + "/CreateAccount";
            public const string UpdateAccount = ControllerBase + "/UpdateAccount";
            //public const string DeleteAccount = ControllerBase + "/DeleteAccount";
            public const string CheckUserNameAvailability = ControllerBase + "/CheckUserNameAvailability";
            public const string AddFavouriteGame = ControllerBase + "/AddFavouriteGame";
            public const string RemoveFavouriteGame = ControllerBase + "/RemoveFavouriteGame";
            //Token Related
            public const string Verify = ControllerBase + "/Verify";
            public const string GetNameFromToken = ControllerBase + "/GetNameFromToken";
        }
         public static class ImageController
        {
            private const string ControllerBase = Base + "/Image";

            //Image Routes
            public const string GetRandomPreview = ControllerBase + "/GetRandomPreview";
            public const string GetPreviewByNum = ControllerBase + "/GetPreviewByNum";
        }

    }
}
