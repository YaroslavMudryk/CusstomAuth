namespace CusstomAuth.Core.Options;

public class DbConnectionOptions
{
    public string ConnectionString { get; set; } = "CusstomAuth.db3";
    public DatabaseProviders DatabaseProvider { get; set; } = DatabaseProviders.Sqlite;
}

public enum DatabaseProviders
{
    Postgres,
    Sqlite,
    SqlServer,
    InMemory
}
