using MediatR;

namespace GameTrove.Application.Query
{
    public class GetLocalFileContents : IRequest<byte[]>
    {
        public string Name { get; set; }
    }
}