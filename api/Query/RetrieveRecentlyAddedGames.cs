using System.Collections.Generic;
using api.ViewModels;
using MediatR;

namespace api.Query
{
    public class RetrieveRecentlyAddedGames : IRequest<IEnumerable<GameViewModel>>
    {
        public int Limit { get; set; }
    }
}