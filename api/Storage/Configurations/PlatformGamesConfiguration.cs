using api.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Storage.Configurations
{
    public class PlatformGamesConfiguration : IEntityTypeConfiguration<PlatformGame>
    {
        public void Configure(EntityTypeBuilder<PlatformGame> builder)
        {
            builder.Property(p => p.Id).HasDefaultValueSql("newid()");
        }
    }
}