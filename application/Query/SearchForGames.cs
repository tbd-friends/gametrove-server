using System.Collections.Generic;
using GameTrove.Application.Commands;
using GameTrove.Application.ViewModels;

namespace GameTrove.Application.Query
{
    public class SearchForGames : AuthenticatedRequest<IEnumerable<GameSearchViewModel>>
    {
        public string Text { get; set; }
    }
}