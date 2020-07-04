using System;

namespace GameTrove.Application.Infrastructure
{
    public interface ITokenService
    {
        string TokenFromGuid(Guid id);
    }
}