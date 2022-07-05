using RazorPagesGB.Models;

namespace RazorPagesGB.Services.EmailService
{
    public interface IEmailService
    {
        public void Send(EmailDto request);

        public Task SendAsync(EmailDto request);
    }
}
