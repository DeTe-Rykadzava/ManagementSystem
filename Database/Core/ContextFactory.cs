using System.Text.Json.Nodes;
using Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Database.Core;

public class ContextFactory : IDesignTimeDbContextFactory<ManagementSystemDatabaseContext>
{
    private const string ConnectionStringFilename = "ConnectionStrings.json";
    private const string ConnectionStringDirectory = "Data";

    private static async Task<JsonNode?> GetFileConnectionNode()
    {
        var path = Path.Combine(ConnectionStringDirectory, ConnectionStringFilename);
        if (!File.Exists(path))
        {
            return null;
        }

        var file = await File.ReadAllTextAsync(path);
        var fileNode = JsonNode.Parse(file);
        return fileNode;
    }

    private static async Task<string> GetConnectionString()
    {
        var file = await GetFileConnectionNode();
        if (file != null)
            return file["Connection"] == null ? throw new NullReferenceException("Connection string is null") : file["Connection"]!.ToString();
        throw new FileNotFoundException("Connection file not found");
    }
    
    public ManagementSystemDatabaseContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

        var connectionString = GetConnectionString().GetAwaiter().GetResult();
        builder.UseNpgsql(connectionString);
        return new ManagementSystemDatabaseContext(builder.Options);
    }
}