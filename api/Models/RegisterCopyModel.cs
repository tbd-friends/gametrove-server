using System;

namespace GameTrove.Api.Models
{
    public class RegisterCopyModel
    {
        public string[] Tags { get; set; }
        public decimal? Cost { get; set; }
        public DateTime? Purchased { get; set; }
    }

    public class UpdateCopyModel : RegisterCopyModel
    {
        public Guid Id { get; set; }
    }
}