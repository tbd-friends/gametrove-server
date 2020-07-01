using System;

namespace GameTrove.Storage.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid TitleId { get; set; }
        public Guid PlatformId { get; set; }
        public string Code { get; set; }
        public DateTime Registered { get; set; }
        public bool IsFavorite { get; set; }
    }
}