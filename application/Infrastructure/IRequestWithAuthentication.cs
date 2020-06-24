using MediatR;

namespace GameTrove.Application.Infrastructure
{
    public interface IBaseRequestWithAuthentication
    {
        string Email { get; set; }
        string Identifier { get; set; }
    }

    public interface IRequestWithAuthentication : IBaseRequestWithAuthentication, IRequest
    {

    }

    public interface IRequestWithAuthentication<out TResponse> : IBaseRequestWithAuthentication, IRequest<TResponse>
    {

    }
}