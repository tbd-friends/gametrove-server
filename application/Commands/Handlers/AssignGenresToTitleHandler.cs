using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class AssignGenresToTitleHandler : IRequestHandler<AssignGenresToTitle>
    {
        private readonly IMediator _mediator;
        private readonly GameTrackerContext _context;

        public AssignGenresToTitleHandler(IMediator mediator, GameTrackerContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public async Task<Unit> Handle(AssignGenresToTitle request, CancellationToken cancellationToken)
        {
            foreach (var name in request.Genres.Distinct())
            {
                var genre = await _mediator.Send(new RegisterGenre { Name = name }, cancellationToken);

                if (_context.TitleGenres
                        .SingleOrDefault(tg => tg.TitleId == request.TitleId && tg.GenreId == genre) ==
                    null)
                {
                    _context.Add(new TitleGenre
                    {
                        TitleId = request.TitleId,
                        GenreId = genre
                    });

                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            return Unit.Value;
        }
    }
}