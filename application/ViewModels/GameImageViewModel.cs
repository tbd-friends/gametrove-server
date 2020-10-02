using System;

namespace GameTrove.Application.ViewModels
{
    public class GameImageViewModel
    {
        public Guid Id { get; set; }
        public bool IsCoverArt { get; set; }
        public string Url { get; set; }
    }
}