using System;

namespace GameTrove.Storage.Models
{
    public class GameImage
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string FileName { get; set; }
        public bool IsCoverArt { get; set; }
    }
}