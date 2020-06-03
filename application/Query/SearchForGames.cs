using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class SearchForGames : IRequest<IEnumerable<GameViewModel>>
    {
        public string Text { get; set; }
        public int? MostRecentlyAdded { get; set; }
    }
}