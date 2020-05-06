using System.Collections.Generic;
using MediatR;

namespace api.Query
{
    public class SearchForGame : IRequest<IEnumerable<string>>, IRequest<Unit>
    {
        public string Text { get; set; }
    }
}