using System;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class RetrieveGameById : IRequestWithAuthentication<GameViewModel>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}