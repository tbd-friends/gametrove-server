using System.Collections;
using System.Collections.Generic;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetGenreLabels : IRequest<IEnumerable<string>>
    {
    }
}