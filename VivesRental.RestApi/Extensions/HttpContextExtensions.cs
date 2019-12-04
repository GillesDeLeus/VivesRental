using System.Linq;
using Microsoft.AspNetCore.Http;

namespace VivesRental.RestApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(c => c.Type == "id").Value;
        }
    }
}
