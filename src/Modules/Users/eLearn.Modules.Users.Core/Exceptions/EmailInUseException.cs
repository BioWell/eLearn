using Shared.Infrastructure.Exceptions;

namespace eLearn.Modules.Users.Core.Exceptions
{
    internal class EmailInUseException : ModularException
    {
        public EmailInUseException() : base("Email is already in use.")
        {
        }
    }
}