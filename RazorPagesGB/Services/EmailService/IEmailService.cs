using RazorPagesGB.DTO;

namespace RazorPagesGB.Services.EmailService
{
    public interface IEmailService
    {
        public void Send(EmailDto request, CancellationToken cancellationToken = default);

        public Task SendAsync(EmailDto request, CancellationToken cancellationToken = default);
    }
}
