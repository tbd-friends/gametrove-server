using System.Collections.Generic;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;

namespace GameTrove.Application.Query
{
    public class SearchForGames : IRequestWithAuthentication<IEnumerable<GameViewModel>>
    {
        public string Text { get; set; }
        public int? MostRecentlyAdded { get; set; }
        public string Identifier { get; set; }
        public string Email { get; set; }
    }
}