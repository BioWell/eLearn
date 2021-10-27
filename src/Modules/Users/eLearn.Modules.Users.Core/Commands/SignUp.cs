using MediatR;

namespace eLearn.Modules.Users.Core.Commands
{
    public class SignUp: IRequest<int>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}