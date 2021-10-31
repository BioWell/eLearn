using Shared.Infrastructure.Exceptions;

namespace eLearn.Modules.Users.Core.Exceptions
{
    internal class PhoneInUseException : ModularException
    {
        public PhoneInUseException() : base("Phone is already in use.")
        {
        }
    }
}