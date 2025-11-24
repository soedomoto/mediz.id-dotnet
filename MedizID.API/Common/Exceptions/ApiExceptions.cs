namespace MedizID.API.Common.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; }

    public ApiException(string message, int statusCode = 400, string errorCode = "BAD_REQUEST")
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public ApiException(string message, Exception innerException, int statusCode = 400, string errorCode = "INTERNAL_SERVER_ERROR")
        : base(message, innerException)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}

public class NotFoundException : ApiException
{
    public NotFoundException(string message)
        : base(message, 404, "NOT_FOUND")
    {
    }
}

public class ValidationException : ApiException
{
    public ValidationException(string message)
        : base(message, 400, "VALIDATION_ERROR")
    {
    }
}

public class UnauthorizedException : ApiException
{
    public UnauthorizedException(string message = "Unauthorized")
        : base(message, 401, "UNAUTHORIZED")
    {
    }
}

public class ForbiddenException : ApiException
{
    public ForbiddenException(string message = "Forbidden")
        : base(message, 403, "FORBIDDEN")
    {
    }
}
