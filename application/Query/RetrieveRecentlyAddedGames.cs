using System.Collections.Generic;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class RetrieveRecentlyAddedGames : IRequest<IEnumerable<GameViewModel>>
    {
        public int Limit { get; set; }
    }
}