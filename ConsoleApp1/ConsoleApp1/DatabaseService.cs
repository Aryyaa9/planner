using Npgsql;

namespace GarageConsoleApp;
public static class DatabaseService
{
    private static NpgsqlConnection? _connection;

    private static string GetConnectionString()
    {
        return @"Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=code_624";
    }

    public static NpgsqlConnection GetSqlConnection()
    {
        if (_connection is null)
        {
            _connection = new NpgsqlConnection(GetConnectionString());
            _connection.Open();
        }

        return _connection;
    }
}
