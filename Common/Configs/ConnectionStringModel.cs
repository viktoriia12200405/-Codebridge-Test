namespace Common.Configs;
public class ConnectionStringModel
{
    private static string _connectionString;

    public ConnectionStringModel(string connectionString)
    {
        _connectionString = connectionString;
    }

    public string ConnectingString => _connectionString;
}
