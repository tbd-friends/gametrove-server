using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Query.Handlers
{
    public class RetrieveGameByCodeHandler : RetrieveGamesBaseHandler<RetrieveGameByCode>
    {
        public RetrieveGameByCodeHandler(GameTrackerContext context) : base(context)
        {

        }

        public override async Task<GameViewModel> Handle(RetrieveGameByCode request, CancellationToken cancellationToken)
        {
            return await GetGames(request.TenantId).SingleOrDefaultAsync(g => g.Code == request.Code, cancellationToken);
        }
    }
}