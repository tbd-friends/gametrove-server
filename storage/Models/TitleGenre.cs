using System;

namespace GameTrove.Storage.Models
{
    public class TitleGenre
    {
        public Guid Id { get; set; }
        public Guid TitleId { get; set; }
        public Guid GenreId { get; set; }
    }
}