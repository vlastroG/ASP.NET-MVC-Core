using RazorPagesGB.DTO;
using RazorPagesGB.Services.EmailService;

namespace RazorPagesGB.Domain.DomainEvents.EventConsumers
{
    public class ProductAddedEmailSender : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger<ProductAddedEmailSender> _logger;


        private readonly string _to = "gordon.ortiz58@ethereal.email";

        public ProductAddedEmailSender(IServiceProvider serviceProvider, ILogger<ProductAddedEmailSender> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            DomainEventsManager.Register<ProductAdded>(ev => _ = SendEmailNotification(ev));
        }

        private async Task SendEmailNotification(ProductAdded ev)
        {
            try
            {
                string body =
                    "<h1>В каталог добавлен следующий товар:</h1>" +
                    ev.Product.ToHTMLString();

                await using (AsyncServiceScope scope = _serviceProvider.CreateAsyncScope())
                {
                    IEmailService emailService =
                        scope.ServiceProvider.GetRequiredService<IEmailService>();

                    await emailService.SendAsync(
                        new EmailDto()
                        {
                            Body = body,
                            Subject = "Добавлен товар",
                            To = _to
                        });
                }
            }
            catch (Exception e)
            {

                _logger.LogError(e, "Error while trying send message.");
            }
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
