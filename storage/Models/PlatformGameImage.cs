using System;

namespace GameTrove.Storage.Models
{
    public class PlatformGameImage
    {
        public Guid Id { get; set; }
        public Guid PlatformGameId { get; set; }
        public string FileName { get; set; }
        public bool IsCoverArt { get; set; }
    }
}