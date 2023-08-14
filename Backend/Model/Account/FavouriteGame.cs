namespace backend.Model.Account
{
    public class FavouriteGame
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int GameId { get; set; }

        public Account Account { get; set; } // Navigationseigenschaft zu Account
    }
}
