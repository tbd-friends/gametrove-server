using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using GameTrove.Storage.Contracts;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterTitleHandler : IRequestHandler<RegisterTitle, TitleViewModel>
    {
        private readonly ITitleRepository _repository;

        public RegisterTitleHandler(ITitleRepository repository)
        {
            _repository = repository;
        }

        public Task<TitleViewModel> Handle(RegisterTitle request, CancellationToken cancellationToken)
        {
            var title = _repository
                .Query(x => x.Name == request.Name && x.Subtitle == request.Subtitle)
                .SingleOrDefault();

            if (title == null)
            {
                title = _repository.AddTitle(request.Name, request.Subtitle);
            }

            return Task.FromResult(new TitleViewModel
            {
                Id = title.Id,
                Name = title.Name,
                Subtitle = title.Subtitle
            });
        }
    }
}