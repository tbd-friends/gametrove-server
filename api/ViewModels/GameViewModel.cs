using System;

namespace api.ViewModels
{
    public class GameViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Registered { get; set; }
    }
}