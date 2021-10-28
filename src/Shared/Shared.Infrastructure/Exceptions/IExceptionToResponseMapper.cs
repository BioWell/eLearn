using System;

namespace Shared.Infrastructure.Exceptions
{
    public interface IExceptionToResponseMapper
    {
        ExceptionResponse? Map(Exception exception);
    }
}