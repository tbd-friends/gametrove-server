using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage;
using GameTrove.Storage.Models;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterTitleHandler : IRequestHandler<RegisterTitle, TitleViewModel>
    {
        private readonly GameTrackerContext _context;

        public RegisterTitleHandler(GameTrackerContext context)
        {
            _context = context;
        }

        public async Task<TitleViewModel> Handle(RegisterTitle request, CancellationToken cancellationToken)
        {
            var title = _context.Titles
                .SingleOrDefault(x => x.Name == request.Name &&
                                      x.Subtitle == request.Subtitle &&
                                      x.TenantId == request.TenantId);

            if (title == null)
            {
                title = new Title { Name = request.Name, Subtitle = request.Subtitle, TenantId = request.TenantId };

                _context.Titles.Add(title);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return new TitleViewModel
            {
                Id = title.Id,
                Name = title.Name,
                Subtitle = title.Subtitle
            };
        }
    }
}