using System.Net;

namespace Shared.Infrastructure.Exceptions
{
    public record ExceptionResponse(object Response, HttpStatusCode StatusCode);
}