using System;
using Microsoft.AspNetCore.SignalR;

namespace api.Models
{
    public class GameModel
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Code { get; set; }
        public Guid Platform { get; set; }
    }

    public class UpdateGameModel : GameModel
    {
        public Guid Id { get; set; }
    }
}