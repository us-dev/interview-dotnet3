using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroceryStoreAPI.ApiResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GroceryStoreAPI.Middleware
{
    /// <summary>
    /// Custom middleware class to handle uncaught global exceptions.
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        /// <summary>
        /// Gets or sets the RequestDelegate.
        /// </summary>
        private RequestDelegate Next { get; set; }

        /// <summary>
        /// Gets or sets the Logger instance.
        /// </summary>
        private ILogger<GlobalExceptionHandlerMiddleware> Logger { get; set; }

        /// <summary>
        /// Default constructor for the <see cref="GlobalExceptionHandlerMiddleware"/>
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            this.Next = next;
            this.Logger = logger;
        }

        /// <summary>
        /// This method continues the middleware pipeline chain.
        /// </summary>
        /// <param name="context">Current HttpContext.</param>
        /// <returns>An async Task.</returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.Next.Invoke(context);
            }
            catch (Exception ex)
            {
                this.Logger.LogCritical(ex, $"UncaughtGlobalException: {ex.Message}");
                var responseBody = new BadRequestModel
                {
                    Error = StatusCodes.Status500InternalServerError.ToString(),
                    Message = "We ran into an error. We will look into the issue!",
                };
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(responseBody));
            }
        }
    }
}
