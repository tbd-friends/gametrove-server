using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class SearchForGame : IRequest<IEnumerable<SearchResultViewModel>>
    {
        public string Text { get; set; }
    }
}