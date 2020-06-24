using System;
using System.Collections.Generic;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;

namespace GameTrove.Application.Query
{
    public class GetCopies : IRequestWithAuthentication<IEnumerable<CopyViewModel>>
    {
        public Guid GameId { get; set; }
        public string Email { get; set; }
        public string Identifier { get; set; }
    }
}