using System.Text.Json.Nodes;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database.Core;

public class ContextFactory : IDesignTimeDbContextFactory<ManagementSystemDatabaseContext>
{
    
    public ManagementSystemDatabaseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

        var connectionString = Settings.GetConnectionString().GetAwaiter().GetResult();
        builder.UseNpgsql(connectionString);
        return new ManagementSystemDatabaseContext(builder.Options);
    }
}