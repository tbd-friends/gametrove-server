using System;
using System.Collections.Generic;
using MediatR;

namespace api.Query
{
    public class GetImageIdentifiersForGame : IRequest<IEnumerable<Guid>>
    {
        public Guid Id { get; set; }
    }
}