using Elastic.Clients.Elasticsearch;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Supportbot.Application.Infrastructure;
using System;

var builder = WebApplication.CreateBuilder(args);
// *************************************************************************************************
// BUILDER CONFIGURATION
// *************************************************************************************************

// Database********** ******************************************************************************
// Read the sql server connection string from appsettings.json located at
// ConnectionStrings -> Default.
builder.Services.AddDbContext<SupportbotContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddScoped(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
        .DefaultIndex("supportdocument-idx");
    return new ElasticsearchClient(settings);
});

builder.Services.AddHttpContextAccessor();     // Required to access the http context in the auth service.
//builder.Services.AddTransient<AuthService>();  // Instantiation on each DI injection.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.OnAppendCookie = cookieContext =>
    {
        cookieContext.CookieOptions.Secure = true;
        cookieContext.CookieOptions.SameSite = builder.Environment.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Strict;
    };
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/login";
    });
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
// Set CORS only in development mode for vue devserver.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueDevserver",
        builder => builder.SetIsOriginAllowed(origin => new System.Uri(origin).IsLoopback)
            .AllowAnyHeader().AllowAnyMethod().AllowCredentials());
});

// *************************************************************************************************
// APP
// *************************************************************************************************
var app = builder.Build();

// SHOW ENVIRONMENT
app.Logger.LogInformation($"ASPNETCORE_ENVIRONMENT is {app.Environment.EnvironmentName}");
app.Logger.LogInformation($"Use Database {builder.Configuration.GetConnectionString("Default")}");

app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowVueDevserver");
}

// Creating the database.
using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<SupportbotContext>())
    {
        await db.CreateDatabaseAsync(isDevelopment: app.Environment.IsDevelopment());
    }
}

app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapRazorPages();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");
app.Run();
