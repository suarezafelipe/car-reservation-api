using System.Net;

namespace API.Middleware;

public class LoggingAndErrorHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingAndErrorHandling> _logger;

    public LoggingAndErrorHandling(RequestDelegate next, ILogger<LoggingAndErrorHandling> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = Guid.NewGuid().ToString();
        SetCorrelationId(context, correlationId);
        LogRequest(context, correlationId);

        try
        {
            await _next(context);
            _logger.LogInformation("Response (CorrelationID: {CorrelationId}): {StatusCode}", correlationId, context.Response.StatusCode);
        }
        catch (Exception ex)
        {
            await HandleUnhandledExceptionAsync(context, correlationId, ex);
        }
    }

    private static void SetCorrelationId(HttpContext context, string correlationId)
    {
        context.TraceIdentifier = correlationId;
        context.Response.Headers.Add("X-Correlation-ID", correlationId);
    }

    private void LogRequest(HttpContext context, string correlationId)
    {
        _logger.LogInformation("Request (CorrelationID: {CorrelationId}): {RequestMethod} {RequestPath}",
            correlationId, context.Request.Method, context.Request.Path);
    }

    private async Task HandleUnhandledExceptionAsync(HttpContext context, string correlationId, Exception ex)
    {
        var requestBody = await GetRequestBodyAsync(context);

        _logger.LogError(ex,
            "An unhandled exception has occurred (CorrelationID: {CorrelationId}). Request: {RequestMethod} {RequestPath} {QueryString} Body: {RequestBody}",
            correlationId, context.Request.Method, context.Request.Path, context.Request.QueryString, requestBody);

        context.Response.Clear();
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            Message = "An unexpected error occurred. Please contact support.",
            CorrelationId = correlationId
        };

        await context.Response.WriteAsJsonAsync(errorResponse);
    }

    private static async Task<string> GetRequestBodyAsync(HttpContext context)
    {
        if (context.Request.Method != HttpMethods.Post && context.Request.Method != HttpMethods.Put)
        {
            return string.Empty;
        }

        context.Request.EnableBuffering();

        using (var streamReader = new StreamReader(context.Request.Body, leaveOpen: true))
        {
            var requestBody = await streamReader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            return requestBody;
        }
    }
}
