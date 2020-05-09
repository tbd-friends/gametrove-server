using api.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Storage
{
    public class GameTrackerContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PlatformGame> PlatformGames { get; set; }
        public DbSet<PlatformGameImage> PlatformGameImages { get; set; }

        public GameTrackerContext(DbContextOptions<GameTrackerContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameTrackerContext).Assembly);
        }
    }
}