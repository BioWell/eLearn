using System;

namespace Shared.Infrastructure.Exceptions
{
    public interface IExceptionCompositionRoot
    {
        ExceptionResponse? Map(Exception exception);
    }
}