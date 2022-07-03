using RazorPagesGB.Configs;
using RazorPagesGB.Services.EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SmtpConfig"));

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
