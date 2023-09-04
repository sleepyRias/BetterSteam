using backend.Model;
using backend.Model.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.DataProvider
{
    public class GameDataProvider : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public IConfiguration Configuration { get; }

        public GameDataProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string gamesConnectionString = Configuration.GetConnectionString("GamesConnection");
            optionsBuilder.UseSqlite(gamesConnectionString);
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
        }
    }
}
