namespace CusstomAuth.Core.Options;

public class DbConnectionOptions
{
    public string ConnectionString { get; set; } = "Host=localhost;Database=postgres;Username=postgres;Password=root;";
    public DatabaseProviders DatabaseProvider { get; set; } = DatabaseProviders.Postgres;
}

public enum DatabaseProviders
{
    Postgres,
    Sqlite,
    SqlServer,
}
