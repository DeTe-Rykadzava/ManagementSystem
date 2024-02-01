using Database.Core;
using Database.Data;
using ManagementSystem.Auth;
using ManagementSystem.Components;
using ManagementSystem.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
// http client для оправки запросов внутри приложения на апи
builder.Services.AddScoped<HttpClient>(sp =>
{
    var httpClient = new HttpClient();
    var server = sp.GetRequiredService<IServer>();
    var addresses = server.Features.Get<IServerAddressesFeature>();
    httpClient.BaseAddress = new Uri(addresses.Addresses.Last());
    return httpClient;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// set authentication by cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.Cookie.Name = "Auth";
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.LoginPath = "/login";
    });
builder.Services.AddScoped<AuthProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<AuthProvider>());
builder.Services.AddAuthenticationCore();

// select server
// PostgreSQL
DatabaseSettings.ChangeSelectedServer(DatabaseServers.PostgreSql);
// MSSQL 
//DatabaseSettings.ChangeSelectedServer(DatabaseServers.Mssql);

builder.Services.AddScoped<ManagementSystemDatabaseContext>(opt => DatabaseSettings.CreateDbContext());

builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var bdContext = scope.ServiceProvider.GetRequiredService<ManagementSystemDatabaseContext>();
    if (bdContext == null)
    {
        app.Logger.LogCritical("Database is not include in DJ");
        Environment.Exit(1);
    }
    else
    {
        bdContext.Database.EnsureCreated();
    }

    
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();