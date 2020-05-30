using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class UpdateTitleHandler : IRequestHandler<UpdateTitle, TitleViewModel>
    {
        private readonly GameTrackerContext _context;

        public UpdateTitleHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<TitleViewModel> Handle(UpdateTitle request, CancellationToken cancellationToken)
        {
            var existing = _context.Titles.Single(t => t.Id == request.TitleId);

            if (!string.IsNullOrEmpty(request.Name))
            {
                existing.Name = request.Name;
                existing.Subtitle = request.Subtitle;

                _context.SaveChanges();
            }

            return Task.FromResult(new TitleViewModel
            {
                Id = existing.Id,
                Name = existing.Name,
                Subtitle = existing.Subtitle
            });
        }
    }
}