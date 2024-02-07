using System.Text.Json.Nodes;
using Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Database.DatabaseCore;

public abstract class DatabaseSettings
{
    private const string ConnectionStringFilename = "ConnectionStrings.json";
    private const string ConnectionStringDirectory = "DataDatabase";

    private static DataBaseServer _currentSelectedServer = DatabaseServers.PostgreSql;

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
            return file[$"{DatabaseServers.PostgreSql.NameConnection}"] == null || file[$"{DatabaseServers.Mssql.NameConnection}"] == null 
                ? throw new NullReferenceException("Connection string is null") 
                : file[$"{_currentSelectedServer.NameConnection}"]!.ToString();
        }
        throw new FileNotFoundException("Connection file not found");
    }
    
    private static JsonNode? GetFileConnectionNodeSync()
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConnectionStringDirectory, ConnectionStringFilename);

        Console.WriteLine($"Path: {path}");
        if (!File.Exists(path))
        {
            return null;
        }

        var file = File.ReadAllText(path);
        var fileNode = JsonNode.Parse(file);
        return fileNode;
    }

    private static string GetConnectionStringSync()
    {
        var file = GetFileConnectionNodeSync();
        if (file != null)
        {
            return file[$"{DatabaseServers.PostgreSql.NameConnection}"] == null || file[$"{DatabaseServers.Mssql.NameConnection}"] == null 
                ? throw new NullReferenceException("Connection string is null") 
                : file[$"{_currentSelectedServer.NameConnection}"]!.ToString();
        }
        throw new FileNotFoundException("Connection file not found");
    }

    public static DbContextOptions<ManagementSystemDatabaseContext> GetContextOptions()
    {
        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

        var connectionString = GetConnectionString().GetAwaiter().GetResult();
        if (_currentSelectedServer == DatabaseServers.PostgreSql)
            builder.UseNpgsql(connectionString);
        else
            builder.UseSqlServer(connectionString);
        return builder.Options;
    }
    
    public static DbContextOptions<ManagementSystemDatabaseContext> GetContextOptionsSync()
    {
        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

        var connectionString = GetConnectionStringSync();
        if (_currentSelectedServer == DatabaseServers.PostgreSql)
            builder.UseNpgsql(connectionString);
        else
            builder.UseSqlServer(connectionString);
        return builder.Options;
    }

    private static ManagementSystemDatabaseContext? _currentContext = null;
    
    public static ManagementSystemDatabaseContext GetDbContext(bool forceReload = false)
    {
        if (_currentContext == null || forceReload)
        {
            var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

            var connectionString = GetConnectionString().GetAwaiter().GetResult();
            if (_currentSelectedServer == DatabaseServers.PostgreSql)
                builder.UseNpgsql(connectionString);
            else
                builder.UseSqlServer(connectionString);
            
            _currentContext = new ManagementSystemDatabaseContext(builder.Options);
        }

        return _currentContext;
    }
    
    public static ManagementSystemDatabaseContext GetDbContext()
    {
        if (_currentContext == null)
        {
            var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

            var connectionString = GetConnectionString().GetAwaiter().GetResult();
            if (_currentSelectedServer == DatabaseServers.PostgreSql)
                builder.UseNpgsql(connectionString);
            else
                builder.UseSqlServer(connectionString);
            
            _currentContext = new ManagementSystemDatabaseContext(builder.Options);
        }

        return _currentContext;
    }
    
    public static ManagementSystemDatabaseContext GetDbContextSync()
    {
        if (_currentContext == null)
        {
            var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();

            var connectionString = GetConnectionStringSync();
            if (_currentSelectedServer == DatabaseServers.PostgreSql)
                builder.UseNpgsql(connectionString);
            else
                builder.UseSqlServer(connectionString);
            
            _currentContext = new ManagementSystemDatabaseContext(builder.Options);
        }

        return _currentContext;
    }
    
    public static ManagementSystemDatabaseContext GetDbContext2()
    {

        var builder = new DbContextOptionsBuilder<ManagementSystemDatabaseContext>();
        var connectionString = GetConnectionString().GetAwaiter().GetResult();
        if (_currentSelectedServer == DatabaseServers.PostgreSql)
            builder.UseNpgsql(connectionString);
        else
            builder.UseSqlServer(connectionString);

        return new ManagementSystemDatabaseContext(builder.Options);
    }
    
}