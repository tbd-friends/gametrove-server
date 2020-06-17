using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class UpdateCopyHandler : IRequestHandler<UpdateCopy, CopyViewModel>
    {
        private readonly GameTrackerContext _context;

        public UpdateCopyHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<CopyViewModel> Handle(UpdateCopy request, CancellationToken cancellationToken)
        {
            var copy = _context.Copies.Single(cp => cp.Id == request.Id);

            copy.Tags = JsonSerializer.Serialize(request.Tags);

            await _context.SaveChangesAsync(cancellationToken);

            return null;
        }
    }
}