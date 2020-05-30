using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Commands
{
    public class RegisterTitle : IRequest<TitleViewModel>
    {
        public string Name { get; set; }
        public string Subtitle { get; set; }
    }
}