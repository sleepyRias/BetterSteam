using backend.Model;
using backend.Model.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.DataProvider
{
    public class SqlDataProvider : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Account> Accounts { get; set; } // Hinzugefügt für die Accounts-Tabelle
        public DbSet<FavouriteGame> FavouriteGames{ get; set; }

        public IConfiguration Configuration { get; }

        public SqlDataProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string gamesConnectionString = Configuration.GetConnectionString("GamesConnection");
            optionsBuilder.UseSqlite(gamesConnectionString);

            string accountsConnectionString = Configuration.GetConnectionString("AccountsConnection");
            optionsBuilder.UseSqlite(accountsConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(g => g.Name).HasColumnName("gamename");
                entity.Property(g => g.GenreId).HasColumnName("genre");
                entity.Property(g => g.Company).HasColumnName("company");
                entity.Property(g => g.Price).HasColumnName("price");
                entity.Property(g => g.ReleaseDate).HasColumnName("release_date");
            });

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
