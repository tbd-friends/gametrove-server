using api.ViewModels;
using MediatR;

namespace api.Commands
{
    public class RetrieveGameByCode : IRequest<GameViewModel>
    {
        public string Code { get; set; }
    }
}