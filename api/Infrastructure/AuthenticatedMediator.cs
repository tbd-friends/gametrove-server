using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using GameTrove.Application.Infrastructure;
using GameTrove.Application.Services;
using GameTrove.Storage;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GameTrove.Api.Infrastructure
{
    public class AuthenticatedMediator : IAuthenticatedMediator
    {
        private readonly IMediator _mediator;
        private readonly AuthenticationService _authentication;
        private readonly ClaimsPrincipal _principal;

        public AuthenticatedMediator(IMediator mediator,
            AuthenticationService authentication,
            IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _authentication = authentication;

            _principal = contextAccessor.HttpContext.User;
        }

        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (request is IRequestWithAuthentication<TResponse> authenticatedRequest)
            {
                var user = _authentication.Get(_principal.Claims.Single(c => c.Type == ClaimTypes.Email).Value);

                authenticatedRequest.UserId = user.UserId;
                authenticatedRequest.TenantId = user.TenantId;

                return await _mediator.Send(authenticatedRequest, cancellationToken);
            }

            return await _mediator.Send(request, cancellationToken);
        }

        public async Task<object> Send(object request, CancellationToken cancellationToken = new CancellationToken())
        {
            if (request is IRequestWithAuthentication authenticatedRequest)
            {
                var user = _authentication.Get(_principal.Claims.Single(c => c.Type == ClaimTypes.Email).Value);

                authenticatedRequest.UserId = user.UserId;
                authenticatedRequest.TenantId = user.TenantId;

                return await _mediator.Send(authenticatedRequest, cancellationToken);
            }

            return await _mediator.Send(request, cancellationToken);
        }

        public Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
        {
            return _mediator.Publish(notification, cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = new CancellationToken()) where TNotification : INotification
        {
            return _mediator.Publish(notification, cancellationToken);
        }
    }
}