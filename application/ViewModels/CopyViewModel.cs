using System;

namespace GameTrove.Application.ViewModels
{
    public class CopyViewModel
    {
        public Guid Id { get; set; }
        public string[] Tags { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Purchased { get; set; }
        public bool IsWanted { get; set; }
    }
}