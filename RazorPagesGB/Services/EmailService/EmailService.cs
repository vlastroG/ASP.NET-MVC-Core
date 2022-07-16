using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using RazorPagesGB.Configs;
using RazorPagesGB.DTO;

namespace RazorPagesGB.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfig _config;

        public EmailService(IOptions<SmtpConfig> options)
        {
            _config = options.Value;
        }

        public void Send(EmailDto request, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.UserName));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.Host, _config.Port, SecureSocketOptions.StartTls, cancellationToken);
            smtp.Authenticate(_config.UserName, _config.Password, cancellationToken);
            smtp.Send(email, cancellationToken);
            smtp.Disconnect(true, cancellationToken);
        }

        public async Task SendAsync(EmailDto request, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.UserName));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.Host, _config.Port, SecureSocketOptions.StartTls, cancellationToken).ConfigureAwait(false);
            await smtp.AuthenticateAsync(_config.UserName, _config.Password, cancellationToken).ConfigureAwait(false);
            await smtp.SendAsync(email, cancellationToken).ConfigureAwait(false);
            await smtp.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
        }
    }
}
