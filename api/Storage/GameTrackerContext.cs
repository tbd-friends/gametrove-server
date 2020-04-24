using api.Storage.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Storage
{
    public class GameTrackerContext : DbContext
    {
        public DbSet<Game> Games { get; set; }

        public GameTrackerContext(DbContextOptions<GameTrackerContext> options) : base(options)
        {

        }
    }
}