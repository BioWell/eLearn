using System.Threading.Tasks;

namespace Shared.Infrastructure.Services.Email
{
    public interface IMailService
    {
        Task SendAsync(MailRequest request);
    }
}