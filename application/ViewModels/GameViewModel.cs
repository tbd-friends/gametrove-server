using System;
using System.Collections.Generic;

namespace GameTrove.Application.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public IEnumerable<string> Genres { get; set; }
        public DateTime Registered { get; set; }
        public string Platform { get; set; }
        public bool IsFavorite { get; set; }
        public decimal? CompleteInBoxPrice { get; set; }
        public decimal? LoosePrice { get; set; }
        public int CopiesOwned { get; set; }
        public IEnumerable<GameImageViewModel> Images { get; set; }

        public GameViewModel()
        {
            Images = new GameImageViewModel[0];
            Genres = new string[0];
        }
    }
}