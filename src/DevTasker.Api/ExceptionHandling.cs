
using DevTasker.Application;

using Microsoft.AspNetCore.Diagnostics;

namespace DevTasker.Api;

public class ExceptionHandling : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is TaskNotFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            return ValueTask.FromResult(true);
        }

        if (exception is UserNotFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

            return ValueTask.FromResult(true);
        }

        return ValueTask.FromResult(false);
    }
}
