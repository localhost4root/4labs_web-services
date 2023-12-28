using FarmEquipmentShop.Models;
using FarmEquipmentShop.Services;
using FarmEquipmentShop.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddViewLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        new CultureInfo("uk")
    };

    options.DefaultRequestCulture = new RequestCulture("uk");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IValidator<AccountModel>, AccountValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseRequestLocalization();

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
        FileStream fileStream = new FileStream(logingPath, FileMode.Append, FileAccess.Write);
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
