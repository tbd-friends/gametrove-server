using System;
using MediatR;

namespace GameTrove.Application.Infrastructure
{
    public interface IBaseRequestWithAuthentication
    {
        Guid UserId { get; set; }
        Guid TenantId { get; set; }
    }

    public interface IRequestWithAuthentication : IBaseRequestWithAuthentication, IRequest
    {

    }

    public interface IRequestWithAuthentication<out TResponse> : IBaseRequestWithAuthentication, IRequest<TResponse>
    {

    }
}