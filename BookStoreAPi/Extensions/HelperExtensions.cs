using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace BookStoreAPi.Extensions
{
    public static class HelperExtensions
    {
        public static long GetUserId(this HttpContext context)
        {
            var idAsString = context.User.Claims.ToList().FirstOrDefault(x => x.Type.Contains("nameidentifier"))?.Value;

            return string.IsNullOrEmpty(idAsString) ? -1 : Convert.ToInt64(idAsString);
        }
    }
}
