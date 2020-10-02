using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Query.Handlers
{
    public class RetrieveGameByIdHandler : RetrieveGamesBaseHandler<RetrieveGameById>
    {
        public RetrieveGameByIdHandler(GameTrackerContext context) : base(context)
        {
        }

        public override async Task<GameViewModel> Handle(RetrieveGameById request, CancellationToken cancellationToken)
        {
            return await GetGames(request.TenantId).SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
        }
    }
}