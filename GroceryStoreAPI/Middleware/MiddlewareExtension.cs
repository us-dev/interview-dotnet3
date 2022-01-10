using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace GroceryStoreAPI.Middleware
{
    /// <summary>
    /// Middleware extension class that provides registration of custom middleware.
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// Extension class for registering global exception handler.
        /// </summary>
        /// <param name="builder"></param>
        public static void UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
