namespace EventsManager.Api.Middleware.ExceptionHandling;

public class ExceptionHandler:IMiddleware
{
    private readonly ILogger<ExceptionHandler> _logger;

    public ExceptionHandler(ILogger<ExceptionHandler> logger)
    {
        _logger= logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }

        catch (Exception exception)
        {
            _logger.LogError(message: exception.Message);
            await ExceptionHandleAsync(context, exception);
        }
    }

    private static Task ExceptionHandleAsync(HttpContext context, Exception exception)
    {
        var details = new ErrorDetails();
        var response = context.Response;
        response.ContentType= "application/json";

        response.StatusCode=exception switch
        {
            InvalidModelException => (int)HttpStatusCode.UnprocessableEntity,
            NoContentException => (int)HttpStatusCode.NotFound,
            InvalidOperationException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError,
        };

        details.Message = exception.Message;
        details.StackTrace=exception.StackTrace!;

        return response.WriteAsync(details.ToString());
    }
}
