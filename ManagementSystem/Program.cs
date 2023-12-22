using Database.Core;
using Database.Data;
using ManagementSystem.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// PostgreSQL
var connectionString = Settings.GetConnectionString(AppDomain.CurrentDomain.BaseDirectory).GetAwaiter().GetResult();
// MSSQL
//var connectionString = Settings.GetConnectionString(AppDomain.CurrentDomain.BaseDirectory, Servers.Mssql).GetAwaiter().GetResult();

builder.Services.AddDbContext<ManagementSystemDatabaseContext>(
    opt => opt.UseNpgsql(connectionString));

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
    var dbContext = scope.ServiceProvider.GetService<ManagementSystemDatabaseContext>();
    dbContext?.Database.EnsureCreated();
    // dbContext?.Roles.Load();
}
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();