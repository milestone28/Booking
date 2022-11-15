using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Booking.Api.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class DateTimeHeader
    {
        private readonly RequestDelegate _next;

        public DateTimeHeader(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            //executed on the request
            httpContext.Request.Headers.Add("my-middleware-header", DateTime.Now.ToString());
            await _next(httpContext);

            // context.Response.Headers.Add("my-middleware-header", DateTime.Now.ToString());
            //executed on the response
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class DateTimeHeaderExtensions
    {
        public static IApplicationBuilder UseDateTimeHeader(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DateTimeHeader>();
        }
    }
}
