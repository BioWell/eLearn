using System;

namespace Shared.Infrastructure.Exceptions
{
    public abstract class ModularException : Exception
    {
        protected ModularException(string message) : base(message)
        {
        }
    }
}