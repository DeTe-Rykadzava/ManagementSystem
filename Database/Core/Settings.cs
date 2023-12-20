using System.Text.Json.Nodes;

namespace Database.Core;

public class Settings
{
    private const string ConnectionStringFilename = "ConnectionStrings.json";
    private const string ConnectionStringDirectory = "Data";
    private static async Task<JsonNode?> GetFileConnectionNode(string? domainDirectory = null)
    {
        var path = string.IsNullOrWhiteSpace(domainDirectory) ? Path.Combine(ConnectionStringDirectory, ConnectionStringFilename) : Path.Combine(domainDirectory, ConnectionStringDirectory, ConnectionStringFilename);

        Console.WriteLine($"Path: {path}");
        if (!File.Exists(path))
        {
            return null;
        }

        var file = await File.ReadAllTextAsync(path);
        var fileNode = JsonNode.Parse(file);
        return fileNode;
    }

    public static async Task<string> GetConnectionString(string? domainDirectory = null)
    {
        var file = await GetFileConnectionNode(domainDirectory);
        if (file != null)
            return file["Connection"] == null ? throw new NullReferenceException("Connection string is null") : file["Connection"]!.ToString();
        throw new FileNotFoundException("Connection file not found");
    }

}