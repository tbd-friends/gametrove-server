using GameTrove.Storage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameTrove.Storage.Configurations
{
    public class GamePricingConfiguration : IEntityTypeConfiguration<GamePricing>
    {
        public void Configure(EntityTypeBuilder<GamePricing> builder)
        {
            builder.ToTable("GamePricing");
        }
    }
}