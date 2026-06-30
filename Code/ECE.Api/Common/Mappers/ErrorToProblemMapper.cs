namespace ECE.Api.Common.Mappers
{
    public static class ErrorToProblemMapper
    {
        public static ProblemDetails Map(List<Error>? errors, HttpContext? context = null)
        {
            if (errors == null || errors.Count == 0)
            {
                return new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unknown error",
                    Type = "https://httpstatuses.com/500",
                    Instance = context?.Request.Path
                };
            }

            var error = errors[0];

            (int statusCode, string title) = error.Type switch
            {
                ErrorKind.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
                ErrorKind.Validation => (StatusCodes.Status400BadRequest, "Bad Request"),
                ErrorKind.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
                ErrorKind.Unauthorized => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                ErrorKind.Forbidden => (StatusCodes.Status403Forbidden, "Forbidden"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
            };

            return new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = error.Description,
                Type = error.Code.ToLower(),
                Instance = context?.Request.Path
            };
        }
    }
}

