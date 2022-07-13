using RazorPagesGB.Configs;
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
    builder.Services.AddControllersWithViews();
    builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));

    builder.Services.AddHostedService<EmailBackgroundService>();
    // ����� ����� - scope, �.�. �� ������ ������ �� ���������� ������
    // �, �������������, �� ������ �� �������� email ����������� �� ����,
    // ����� ��������� ������ ���� ��������� ������ ������� �������� ���������.
    // �� ���� ������ ����� ���������� �������� 1 ����� � ������ � ��� 1 ���������.
    builder.Services.AddScoped<IEmailService, EmailService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
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
