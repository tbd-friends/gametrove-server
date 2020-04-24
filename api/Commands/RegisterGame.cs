using MediatR;

namespace api.Commands
{
    public class RegisterGame : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}