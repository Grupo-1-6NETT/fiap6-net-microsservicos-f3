using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;


public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenBucketRateLimiter _limiter;

    public RateLimitingMiddleware(RequestDelegate next)
    {
        _next = next;

        // Configura um Rate Limit de 5 requisições a cada 10 segundos
        _limiter = new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
        {
            TokenLimit = 5, // Máximo de requisições permitidas
            TokensPerPeriod = 5, // Requisições liberadas a cada período
            ReplenishmentPeriod = TimeSpan.FromSeconds(30), // Tempo para renovar os tokens
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0 // Sem fila de espera
        });
    }

    public async Task InvokeAsync(HttpContext context)
    {
        using var lease = await _limiter.AcquireAsync(1);

        if (!lease.IsAcquired)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Limite de requisição atingido.");
            return;
        }

        await _next(context);
    }
}
