using System;

namespace GameTrove.Application.ViewModels
{
    public class GameViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Registered { get; set; }
        public string Platform { get; set; }
    }
}