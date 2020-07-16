using System;

namespace GameTrove.Storage.Models
{
    public class GamePricing
    {
        public Guid Id { get; set; }
        public int PriceChartingId { get; set; }
        public decimal CompleteInBoxPrice { get; set; }
        public decimal LoosePrice { get; set; }
        public short MatchConfidence { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}