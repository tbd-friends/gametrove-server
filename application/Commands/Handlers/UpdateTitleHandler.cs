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
        private readonly IMediator _mediator;
        private readonly GameTrackerContext _context;

        public UpdateTitleHandler(IMediator mediator, GameTrackerContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<TitleViewModel> Handle(UpdateTitle request, CancellationToken cancellationToken)
        {
            var existing = _context.Titles.Single(t => t.Id == request.TitleId);

            if (!string.IsNullOrEmpty(request.Name))
            {
                existing.Name = request.Name;
                existing.Subtitle = request.Subtitle;

                await _context.SaveChangesAsync(cancellationToken);
            }

            if (request.Genres != null)
            {
                await _mediator.Send(new AssignGenresToTitle
                    {
                        TitleId = existing.Id, 
                        Genres = request.Genres
                    },
                    cancellationToken);
            }

            return new TitleViewModel
            {
                Id = existing.Id,
                Name = existing.Name,
                Subtitle = existing.Subtitle
            };
        }
    }
}