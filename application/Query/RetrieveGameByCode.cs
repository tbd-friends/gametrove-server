using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class RetrieveGameByCode : IRequest<GameViewModel>
    {
        public string Code { get; set; }
    }
}