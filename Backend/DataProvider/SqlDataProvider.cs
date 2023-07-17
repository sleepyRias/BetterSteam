using backend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace backend.DataProvider
{
    public class SqlDataProvider : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public IConfiguration Configuration { get; }

        public SqlDataProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlite(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(g => g.Name)
                      .HasColumnName("gamename");

                entity.Property(g => g.GenreId)
                      .HasColumnName("genre");

                entity.Property(g => g.Company)
                      .HasColumnName("company");

                entity.Property(g => g.Price)
                      .HasColumnName("price");

                entity.Property(g => g.ReleaseDate)
                      .HasColumnName("release_date");
            });
            // Hier kannst du benutzerdefinierte Konfigurationen für deine Entitäten hinzufügen, falls erforderlich
        }
    }
}
