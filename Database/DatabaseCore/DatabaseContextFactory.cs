using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database.DatabaseCore;

internal class DatabaseContextFactory : IDesignTimeDbContextFactory<ManagementSystemDatabaseContext>
{
    public ManagementSystemDatabaseContext CreateDbContext(string[] args)
    {
        // select database server
        DatabaseSettings.ChangeSelectedServer(DatabaseServers.Mssql);
        //DatabaseSettings.ChangeSelectedServer(DatabaseServers.PostgreSql);
        
        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

        var connectionString = DatabaseSettings.GetConnectionString().GetAwaiter().GetResult();
        
        // configure builder for selected server
        builder.UseSqlServer(connectionString);
        //builder.UseNpgsql(connectionString);
        
        return new ManagementSystemDatabaseContext(builder.Options);
    }
}