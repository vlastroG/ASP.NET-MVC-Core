using RazorPagesGB.Models;
using RazorPagesGB.Services.EmailService;
using System.Diagnostics;

namespace RazorPagesGB.Services.EmailBackgroundService
{
    public class EmailBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly string _to = "gordon.ortiz58@ethereal.email";


        public EmailBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(TimeSpan.FromMinutes(60));
            Stopwatch sw = Stopwatch.StartNew();
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                string body = "<h1>Server works normal.<h1>";
                await using (AsyncServiceScope scope = _serviceProvider.CreateAsyncScope())
                {
                    IEmailService emailService =
                        scope.ServiceProvider.GetRequiredService<IEmailService>();

                    await emailService.SendAsync(
                        new EmailDto()
                        {
                            Body = body,
                            Subject = "Работа сервера",
                            To = _to
                        },
                    stoppingToken);
                }
            }
        }
    }
}
