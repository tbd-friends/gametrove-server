using GameTrove.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameTrove.Storage.Configurations
{
    public class PlatformGameImageConfiguration : IEntityTypeConfiguration<GameImage>
    {
        public void Configure(EntityTypeBuilder<GameImage> builder)
        {
            builder.Property(p => p.Id).HasDefaultValueSql("newid()");
        }
    }
}