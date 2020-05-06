using api.ViewModels;
using MediatR;

namespace api.Query
{
    public class RetrieveGameByCode : IRequest<GameViewModel>
    {
        public string Code { get; set; }
    }
}