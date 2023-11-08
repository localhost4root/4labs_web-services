using FarmEquipmentShop.Services;
using Microsoft.AspNetCore.Http.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();

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

app.Use((context, next) =>
{
    var logingPath = app.Configuration["LogingPath"];

    if (logingPath == null)
    {
        throw new Exception("Cannot load LogingPath property");
    }

    if (!File.Exists(logingPath))
    {
        using (StreamWriter streamWr = File.CreateText(logingPath))
        {
            var requestUrl = context.Request.GetDisplayUrl();
            var requestDateTime = DateTime.Now.ToString();
            var requestIp = context.Connection.RemoteIpAddress?.ToString();

            streamWr.WriteLine(
                "Request url: {0}, request date/time: {1}, request IP: {2}",
                requestUrl,
                requestDateTime,
                requestIp);

            streamWr.Close();
        }
    }
    else
    {
        using (FileStream fileStream = new FileStream(logingPath, FileMode.Append, FileAccess.Write))
        using (StreamWriter streamWr = new StreamWriter(fileStream))
        {
            var requestUrl = context.Request.GetDisplayUrl();
            var requestDateTime = DateTime.Now.ToString();
            var requestIp = context.Connection.RemoteIpAddress?.ToString();

            

            streamWr.WriteLine(
                "Request url: {0}, request date/time: {1}, request IP: {2}",
                requestUrl,
                requestDateTime,
                requestIp);

            streamWr.Close();
            fileStream.Close();
        }
    }

    return next();
});

app.Run();
