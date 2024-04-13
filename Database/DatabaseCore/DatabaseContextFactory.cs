using Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database.DatabaseCore;

internal class DatabaseContextFactory : IDesignTimeDbContextFactory<ManagementSystemDatabaseContext>
{
    public ManagementSystemDatabaseContext CreateDbContext(string[] args)
    {
        // select database server
        // DatabaseSettings.ChangeSelectedServer(DatabaseServers.Mssql);
        DatabaseSettings.ChangeSelectedServer(DatabaseServers.PostgreSql);

        try
        {
            var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

            var connectionString = DatabaseSettings.GetConnectionString().GetAwaiter().GetResult();
            Console.WriteLine(connectionString);
        
            // configure builder for selected server
            // builder.UseSqlServer(connectionString);
            builder.UseNpgsql(connectionString);
        
            return new ManagementSystemDatabaseContext(builder.Options);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + "\n" + e.InnerException);
            throw;
        }
    }
}