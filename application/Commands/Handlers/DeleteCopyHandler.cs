using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class DeleteCopyHandler : IRequestHandler<DeleteCopy>
    {
        private readonly GameTrackerContext _context;

        public DeleteCopyHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<Unit> Handle(DeleteCopy request, CancellationToken cancellationToken)
        {
            var copy = _context.Copies.Single(c => c.Id == request.CopyId);

            _context.Remove(copy);

            _context.SaveChanges();

            return Unit.Task;
        }
    }
}