using System;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.ViewModels;
using MediatR;

namespace GameTrove.Application.Query
{
    public class RetrieveGameByCode : IRequestWithAuthentication<GameViewModel>
    {
        public string Code { get; set; }
        public Guid UserId { get; set; }
        public Guid TenantId { get; set; }
    }
}