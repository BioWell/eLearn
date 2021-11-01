using System.Collections.Generic;
using System.Net;
using Shared.Infrastructure.Exceptions;

namespace eLearn.Modules.Users.Core.Exceptions
{
    internal class IdentityException: CustomException
    {
        public IdentityException(string message, List<string> errors = default!, HttpStatusCode statusCode = default)
            : base(message, errors, statusCode)
        {
        }
    }
}