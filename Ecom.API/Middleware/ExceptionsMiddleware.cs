using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionsMiddleware(RequestDelegate next, IHostEnvironment hostEnvironment, IMemoryCache memoryCache)
        {
            _next = next;
            _hostEnvironment = hostEnvironment;
            _memoryCache = memoryCache;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Apply security headers
                ApplySecurity(context);

                if (IsRequestAllowed(context) == false)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiExceptions((int)HttpStatusCode.TooManyRequests, "Rate limit exceeded. Please try again later.");
                    await context.Response.WriteAsJsonAsync(response);

                }
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _hostEnvironment.IsDevelopment() ?
                    new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString()) :
                    new ApiExceptions((int)HttpStatusCode.InternalServerError, ex.Message);

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);

            }
        }

        private bool IsRequestAllowed(HttpContext context)
        {
            // Implement your logic to check if the request is allowed
            var ip = context.Connection.RemoteIpAddress.ToString();
            var cacheKey = $"Rate:{ip}";
            var dateNow = DateTime.Now;
            var (timeTamp, count) = _memoryCache.GetOrCreate(cacheKey, entry =>

            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timeTamp: dateNow, count: 0);
            });



            // For example, you can check the request path, method, headers, etc.
            if (dateNow - timeTamp < _rateLimitWindow)
            {
                // Reset the count if the time window has passed
                if (count > 8)
                {
                    return false; // Rate limit exceeded
                }
                _memoryCache.Set(cacheKey, (timeTamp, count += 1), _rateLimitWindow);

            }
            else
            {
                // Increment the count if within the time window
                _memoryCache.Set(cacheKey, (timeTamp, count), _rateLimitWindow);
            }
            return true; // Placeholder for actual logic
        }

        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"]= "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";


        }


    }
}