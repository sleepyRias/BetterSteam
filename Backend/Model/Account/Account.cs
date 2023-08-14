﻿namespace backend.Model.Account
{
    public class Account
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? PasswordHash { get; set; }
        public string? Theme { get; set; }
        public List<FavouriteGame>? FavouriteGames { get; set; }
    }

}
