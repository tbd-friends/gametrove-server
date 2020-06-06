using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterGenreHandler : IRequestHandler<RegisterGenre, Guid>
    {
        private readonly GameTrackerContext _context;

        public RegisterGenreHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public Task<Guid> Handle(RegisterGenre request, CancellationToken cancellationToken)
        {
            var existing = _context.Genres.SingleOrDefault(g => g.Name == request.Name);

            if (existing == null)
            {
                var genre = new Genre { Name = request.Name };

                _context.Genres.Add(genre);

                _context.SaveChanges();

                return Task.FromResult(genre.Id);
            }

            return Task.FromResult(existing.Id);
        }
    }
}