using Database.Core;
using Database.Data;
using ManagementSystem.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// select server
// PostgreSQL
DatabaseSettings.ChangeSelectedServer(DatabaseServers.PostgreSql);
// MSSQL 
DatabaseSettings.ChangeSelectedServer(DatabaseServers.Mssql);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// check database connection
ManagementSystemDatabaseContext.Context.Database.EnsureCreated();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();