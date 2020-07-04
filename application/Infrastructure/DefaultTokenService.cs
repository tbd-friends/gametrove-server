using System;
using System.Text;

namespace GameTrove.Application.Infrastructure
{
    public class DefaultTokenService : ITokenService
    {
        private readonly int _length;

        public DefaultTokenService() : this(5)
        { }

        public DefaultTokenService(int length)
        {
            _length = length;
        }

        public string TokenFromGuid(Guid id)
        {
            var random = new Random(Guid.NewGuid().GetHashCode());

            var sb = new StringBuilder();

            string source = id.ToString().Replace("-", "");

            for (int idx = 0; idx < _length; idx++)
            {
                sb.Append(source[random.Next(source.Length)]);
            }

            return sb.ToString();
        }
    }
}