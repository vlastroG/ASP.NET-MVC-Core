using Microsoft.AspNetCore.HttpLogging;
using RazorPagesGB.Configs;
using RazorPagesGB.Domain.DomainEvents.EventConsumers;
using RazorPagesGB.Services.EmailBackgroundService;
using RazorPagesGB.Services.EmailService;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
Log.Information("Starting up");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((ctx, conf) =>
    {
        conf
        .MinimumLevel.Debug()
        .WriteTo.File("log-.txt", rollingInterval: RollingInterval.Day)
        .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
        .ReadFrom.Configuration(ctx.Configuration);
    });

    // Add services to the container.
    builder.Services.AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.RequestHeaders
                                | HttpLoggingFields.RequestBody
                                | HttpLoggingFields.ResponseHeaders
                                | HttpLoggingFields.ResponseBody;
    });
    builder.Services.AddControllersWithViews();
    builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));

    builder.Services.AddHostedService<EmailBackgroundService>();
    builder.Services.AddHostedService<ProductAddedEmailSender>();
    // Время жизни - scope, т.к. на каждый запрос на добавление товара
    // и, следовательно, на запрос по отправке email уведомления об этом,
    // нужно создавать только один экземпляр класса сервиса отправки сообщений.
    // За один запрос может добавиться максимум 1 товар и вместе с ним 1 сообщение.
    builder.Services.AddScoped<IEmailService, EmailService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    app.UseHttpLogging();

    app.UseHttpsRedirection();

    app.Use(async (HttpContext context, Func<Task> next) =>
    {
        var userAgent = context.Request.Headers.UserAgent.ToString().ToLower();
        if (!userAgent.Contains("edg"))
        {
            context.Response.ContentType = "text/html; charset=UTF-8";
            await context.Response.WriteAsync(
                "<h1>Your browser is not suppored:(</h1>" +
                "<h2>Try <a href=\"https://www.microsoft.com/en-us/edge\">Microsoft Edge :D</a></h2>");
            return;
        }
        await next();
    });

    app.UseStaticFiles();
    app.UseSerilogRequestLogging();

    app.UseRouting();

    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Server crashed!");
}
finally
{
    Log.Information("Shut doen complete.");
    Log.CloseAndFlush();
}
