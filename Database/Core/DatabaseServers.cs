namespace Database.Core;

public abstract class DatabaseServers
{
    public static DataBaseServer PostgreSql { get; } = new DataBaseServer("NPGSQLConnection");
    public static DataBaseServer Mssql { get; } = new DataBaseServer("MSSQLConnection");
}

public class DataBaseServer
{
    public string NameConnection { get; }

    public DataBaseServer(string nameConnection)
    {
        NameConnection = nameConnection;
    }
}