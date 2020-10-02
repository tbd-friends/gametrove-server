using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using MediatR;

namespace GameTrove.Application.Query.Handlers
{
    public abstract class RetrieveGamesBaseHandler<THandler> : IRequestHandler<THandler, GameViewModel>
        where THandler : IRequest<GameViewModel>
    {
        protected GameTrackerContext Context { get; }

        protected RetrieveGamesBaseHandler(GameTrackerContext context)
        {
            Context = context;
        }

        public abstract Task<GameViewModel> Handle(THandler request, CancellationToken cancellationToken);

        protected IQueryable<GameViewModel> GetGames(Guid tenantId)
        {
            return from pg in Context.Games
                   join p in Context.Platforms on pg.PlatformId equals p.Id
                   join t in Context.Titles on pg.TitleId equals t.Id
                   join gp in Context.GamePricing on pg.Id equals gp.Id
                       into gpx
                   from pricing in gpx.DefaultIfEmpty()
                   where pg.TenantId == tenantId
                   select new GameViewModel
                   {
                       Id = pg.Id,
                       Name = t.Name,
                       Subtitle = t.Subtitle,
                       Registered = pg.Registered,
                       Code = pg.Code,
                       Platform = p.Name,
                       IsFavorite = pg.IsFavorite,
                       CompleteInBoxPrice = pricing.CompleteInBoxPrice,
                       LoosePrice = pricing.LoosePrice,
                       CopiesOwned = (from c in Context.Copies where c.GameId == pg.Id && c.TenantId == pg.TenantId select c).Count(),
                       Images = (from pgi in Context.PlatformGameImages
                                 where pgi.GameId == pg.Id
                                 select new GameImageViewModel
                                 {
                                     Id = pgi.Id,
                                     IsCoverArt = pgi.IsCoverArt,
                                     Url = $"images/{pgi.Id}"
                                 }),
                       Genres = (from tg in Context.TitleGenres
                                 join g in Context.Genres on tg.GenreId equals g.Id
                                 where tg.TitleId == t.Id
                                 select g.Name).ToList()
                   };
        }
    }
}