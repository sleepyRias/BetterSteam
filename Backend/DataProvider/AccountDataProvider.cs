using backend.Model;
using backend.Model.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace backend.DataProvider
{
    public class AccountDataProvider : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<FavouriteGame> FavouriteGames { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        public IConfiguration Configuration { get; }

        public AccountDataProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string accountsConnectionString = Configuration.GetConnectionString("AccountsConnection");
            optionsBuilder.UseSqlite(accountsConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordSalt).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Theme);
            });

            modelBuilder.Entity<FavouriteGame>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountId);
                entity.Property(e => e.GameId);
            });

            modelBuilder.Entity<Wishlist>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountId);
                entity.Property(e => e.GameId);
                entity.ToTable("Wishlist");
            });
        }
    }
}
