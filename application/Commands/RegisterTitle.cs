using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterTitle : AuthenticatedRequest<TitleViewModel>
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
    }
}