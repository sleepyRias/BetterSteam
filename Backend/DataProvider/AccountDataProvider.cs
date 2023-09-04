using backend.Model;
using backend.Model.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.DataProvider
{
    public class AccountDataProvider : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<FavouriteGame> FavouriteGames { get; set; }

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

            modelBuilder.Entity<FavouriteGame>()
                .HasKey(fg => new { fg.AccountId, fg.GameId });

            modelBuilder.Entity<FavouriteGame>()
                .HasOne(fg => fg.Account) // Navigationseigenschaft zu Account
                .WithMany(a => a.FavouriteGames) // Navigationseigenschaft zu FavouriteGames in Account
                .HasForeignKey(fg => fg.AccountId);


            modelBuilder.Entity<Account>()
                .HasMany(a => a.FavouriteGames)
                .WithOne()
                .HasForeignKey(fg => fg.AccountId);
        }
    }
}
