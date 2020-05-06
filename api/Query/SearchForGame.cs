using System.Collections.Generic;
using api.ViewModels;
using MediatR;

namespace api.Query
{
    public class SearchForGame : IRequest<IEnumerable<SearchResultViewModel>>
    {
        public string Text { get; set; }
    }
}