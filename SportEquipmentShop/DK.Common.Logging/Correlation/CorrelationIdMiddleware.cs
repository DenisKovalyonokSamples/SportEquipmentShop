using DK.Common.Logging.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace DK.Common.Logging.Correlation
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        private const string _correlationIdHeader = "X-Correlation-Id";

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            var correlationId = GetCorrelationId(context, correlationIdGenerator);
            AddcorrelationIdHeader(context, correlationId);
            await _next(context);
        }

        private static StringValues GetCorrelationId(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            {
                correlationIdGenerator.Set(correlationId);
                return correlationId;
            }
            else
            {
                return correlationIdGenerator.Get();
            }
        }

        private static void AddcorrelationIdHeader(HttpContext context, StringValues correlationId)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Append(_correlationIdHeader, new[] { correlationId.ToString() });
                return Task.CompletedTask;
            });
        }
    }
}
