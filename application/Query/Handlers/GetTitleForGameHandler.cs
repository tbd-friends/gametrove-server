using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GameTrove.Application.Query.Handlers
{
    public class GetTitleForGameHandler : IRequestHandler<GetTitleForGame, TitleViewModel>
    {
        private readonly GameTrackerContext _context;

        public GetTitleForGameHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<TitleViewModel> Handle(GetTitleForGame request, CancellationToken cancellationToken)
        {
            var result = (from g in _context.Games
                          join t in _context.Titles on g.TitleId equals t.Id
                          where g.Id == request.GameId
                          select new TitleViewModel
                          {
                              Id = t.Id,
                              Name = t.Name,
                              Subtitle = t.Subtitle,
                              Genres = (from tg in _context.TitleGenres
                                        join g in _context.Genres on tg.GenreId equals g.Id
                                        where tg.TitleId == t.Id
                                        select g.Name)
                          });

            return await result.SingleOrDefaultAsync(cancellationToken);
        }
    }
}