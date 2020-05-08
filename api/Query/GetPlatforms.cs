using System.Collections.Generic;
using api.ViewModels;
using MediatR;

namespace api.Query
{
    public class GetPlatforms : IRequest<IEnumerable<PlatformViewModel>>
    {

    }
}