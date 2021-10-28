using Shared.Infrastructure.Exceptions;

namespace eLearn.Modules.Users.Core.Exceptions
{
    internal class InvalidEmailException : ModularException
    {
        public string Email { get; }

        public InvalidEmailException(string email) : base($"State is invalid: '{email}'.")
        {
            Email = email;
        }
    }
}