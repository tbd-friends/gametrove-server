using MediatR;

namespace GameTrove.Application.Commands
{
    public class VerifyUser : IRequest<bool>
    {
        public string Email { get; set; }
    }
}