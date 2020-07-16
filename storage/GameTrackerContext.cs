using GameTrove.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Storage
{
    public class GameTrackerContext : DbContext
    {
        public DbSet<Copy> Copies { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePricing> GamePricing { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<GameImage> PlatformGameImages { get; set; }
        public DbSet<TenantInvite> TenantInvites { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<User> Users { get; set; }

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