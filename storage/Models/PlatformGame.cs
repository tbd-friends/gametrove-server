using System;

namespace GameTrove.Storage.Models
{
    public class PlatformGame
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public Guid PlatformId { get; set; }
        public string Code { get; set; }
        public DateTime Registered { get; set; }
    }
}