using System;

namespace GameTrove.Storage.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Identifier { get; set; }
    }
}