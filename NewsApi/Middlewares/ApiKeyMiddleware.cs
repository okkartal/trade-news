namespace NewsApi.Middlewares;

public class ApiKeyMiddleware
{
    private const string ApiKeyName = "ApiKey";
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Api Key was not provided. (Using ApiKeyMiddleware)");
            return;
        }

        var configuration = context.RequestServices.GetRequiredService<IConfiguration>();

        var apiKey = configuration[ApiKeyName];

        if (apiKey != null && !apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client. (Using ApiKeyMiddleware)");
            return;
        }

        await _next(context);
    }
}