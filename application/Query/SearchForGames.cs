using System.Collections.Generic;
using GameTrove.Application.Commands;
using GameTrove.Application.ViewModels;

namespace GameTrove.Application.Query
{
    public class SearchForGames : AuthenticatedRequest<IEnumerable<GameViewModel>>
    {
        public string Text { get; set; }
        public int? MostRecentlyAdded { get; set; }
    }
}