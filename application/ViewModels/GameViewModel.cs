using System;

namespace GameTrove.Application.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public DateTime Registered { get; set; }
        public string Platform { get; set; }
        public bool IsFavorite { get; set; }
    }
}