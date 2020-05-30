using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class GetPlatforms : IRequest<IEnumerable<PlatformViewModel>>
    {

    }
}