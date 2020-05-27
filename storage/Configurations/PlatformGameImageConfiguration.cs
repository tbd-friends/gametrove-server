using GameTrove.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameTrove.Storage.Configurations
{
    public class PlatformGameImageConfiguration : IEntityTypeConfiguration<PlatformGameImage>
    {
        public void Configure(EntityTypeBuilder<PlatformGameImage> builder)
        {
            builder.Property(p => p.Id).HasDefaultValueSql("newid()");
        }
    }
}