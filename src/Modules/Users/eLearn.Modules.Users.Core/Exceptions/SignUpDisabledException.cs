using Shared.Infrastructure.Exceptions;

namespace eLearn.Modules.Users.Core.Exceptions
{
    internal class SignUpDisabledException : ModularException
    {
        public SignUpDisabledException() : base("Sign up is disabled.")
        {
        }
    }
}