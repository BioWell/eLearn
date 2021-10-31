using System;

namespace Shared.Infrastructure.Services.Email
{
    public class MailRequest
    {
        public string To { get; set; } = String.Empty;

        public string Subject { get; set; } = String.Empty;

        public string Body { get; set; } = String.Empty;

        public string From { get; set; } = String.Empty;
    }
}