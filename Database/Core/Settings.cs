using System.Text.Json.Nodes;
using Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Database.Core;

public class Settings
{
    private const string ConnectionStringFilename = "ConnectionStrings.json";
    private const string ConnectionStringDirectory = "Data";

    private static DataBaseServer _currentSelectedServer = Servers.PostgreSql;

    public static void ChangeSelectedServer(DataBaseServer server)
    {
        _currentSelectedServer = server;
    }

    private static async Task<JsonNode?> GetFileConnectionNode()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConnectionStringDirectory, ConnectionStringFilename);

        Console.WriteLine($"Path: {path}");
        if (!File.Exists(path))
        {
            return null;
        }

        var file = await File.ReadAllTextAsync(path);
        var fileNode = JsonNode.Parse(file);
        return fileNode;
    }

    public static async Task<string> GetConnectionString()
    {
        var file = await GetFileConnectionNode();
        if (file != null)
        {
            return file[$"{Servers.PostgreSql.NameConnection}"] == null || file[$"{Servers.Mssql.NameConnection}"] == null 
                ? throw new NullReferenceException("Connection string is null") 
                : file[$"{_currentSelectedServer.NameConnection}"]!.ToString();
        }
        throw new FileNotFoundException("Connection file not found");
    }

    public static ManagementSystemDatabaseContext CreateDbContext()
    {
        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

        var connectionString = GetConnectionString().GetAwaiter().GetResult();
        if (_currentSelectedServer == Servers.PostgreSql)
            builder.UseNpgsql(connectionString);
        else
            builder.UseSqlServer(connectionString);
        return new ManagementSystemDatabaseContext(builder.Options);
    }
    
}