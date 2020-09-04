using System;

namespace GameTrove.Api.Models
{
    public class FavoriteModel
    {
        public Guid GameId { get; set; }   
    }

    public class CoverArtModel
    {
        public Guid ImageId { get; set; }
    }
}