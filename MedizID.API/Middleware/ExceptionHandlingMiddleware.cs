using System.Net;
using System.Text.Json;
using MedizID.API.Common.Exceptions;
using MedizID.API.DTOs;

namespace MedizID.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception has occurred");
            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse();
        var status = HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case NotFoundException notFoundException:
                status = HttpStatusCode.NotFound;
                response.ErrorCode = notFoundException.ErrorCode;
                response.Message = notFoundException.Message;
                break;

            case ValidationException validationException:
                status = HttpStatusCode.BadRequest;
                response.ErrorCode = validationException.ErrorCode;
                response.Message = validationException.Message;
                break;

            case UnauthorizedException unauthorizedException:
                status = HttpStatusCode.Unauthorized;
                response.ErrorCode = unauthorizedException.ErrorCode;
                response.Message = unauthorizedException.Message;
                break;

            case ForbiddenException forbiddenException:
                status = HttpStatusCode.Forbidden;
                response.ErrorCode = forbiddenException.ErrorCode;
                response.Message = forbiddenException.Message;
                break;

            case ApiException apiException:
                status = (HttpStatusCode)apiException.StatusCode;
                response.ErrorCode = apiException.ErrorCode;
                response.Message = apiException.Message;
                break;

            default:
                response.ErrorCode = "INTERNAL_SERVER_ERROR";
                response.Message = "An unexpected error occurred";
                response.Details = exception.Message;
                break;
        }

        context.Response.StatusCode = (int)status;

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);
    }
}
