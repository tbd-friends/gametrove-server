using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands.Handlers
{
    public class RegisterTitleHandler : IRequestHandler<RegisterTitle, TitleViewModel>
    {
        public Task<TitleViewModel> Handle(RegisterTitle request, CancellationToken cancellationToken)
        {
            return null;
        }
    }
}