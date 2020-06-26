using System;

namespace GameTrove.Api.Models
{
    public class AddCopyModel
    {
        public string[] Tags { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Purchased { get; set; }
        public bool IsWanted { get; set; }
    }

    public class UpdateCopyModel : AddCopyModel
    {
        public Guid Id { get; set; }
    }
}