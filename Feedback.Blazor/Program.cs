using Feedback.Blazor.Utilities;
using Feedback.Models;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["StorageConnectionString:blob"], preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["StorageConnectionString:queue"], preferMsi: true);
});
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<GoogleReCaptchaOptions>(builder.Configuration.GetSection(nameof(GoogleReCaptchaOptions)));
builder.Services.AddScoped(sp =>
{
    var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
    var country = "LV";

    var countryValue = httpContextAccessor.HttpContext?.Request.Headers["X-Office-Country"];

    if (countryValue?.Any() ?? false)
    {
        country = countryValue.Value.FirstOrDefault()?.ToUpper();
    }

    var config = sp.GetRequiredService<IConfiguration>();
    var options = new FeedbackOptions();

    config.GetSection($"{nameof(FeedbackOptions)}-{country}").Bind(options);

    return options;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
