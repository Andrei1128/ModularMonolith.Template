using Core.Common.Models;

namespace Core.Infrastructure;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(title: GetTitle(result.Error),
                               detail: GetDetail(result.Error),
                               type: GetType(result.Error.ErrorType),
                               statusCode: GetStatusCode(result.Error.ErrorType),
                               extensions: GetErrors(result));
    }

    private static string GetTitle(Error error) => error.ErrorType switch
    {
        Error.Type.Validation => error.Code,
        Error.Type.Problem => error.Code,
        Error.Type.NotFound => error.Code,
        Error.Type.Conflict => error.Code,
        _ => "Server failure"
    };

    private static string GetDetail(Error error) => error.ErrorType switch
    {
        Error.Type.Validation => error.Description,
        Error.Type.Problem => error.Description,
        Error.Type.NotFound => error.Description,
        Error.Type.Conflict => error.Description,
        _ => "An unexpected error occurred"
    };

    private static string GetType(Error.Type errorType) => errorType switch
    {
        Error.Type.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        Error.Type.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
        Error.Type.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
        Error.Type.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
        _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
    };

    private static int GetStatusCode(Error.Type errorType) => errorType switch
    {
        Error.Type.Validation => StatusCodes.Status400BadRequest,
        Error.Type.NotFound => StatusCodes.Status404NotFound,
        Error.Type.Conflict => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };

    private static Dictionary<string, object?>? GetErrors(Result result)
    {
        if (result.Error is not ValidationError validationError)
        {
            return null;
        }

        return new Dictionary<string, object?>
        {
            { "errors", validationError.Errors }
        };
    }
}
