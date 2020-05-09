using Microsoft.AspNetCore.Http;

namespace api.Infrastructure
{
    public static class UrlExtensions
    {
        public static string GetHost(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}";
        }
    }
}