using MySqlConnector;
using EmployeeManagementAPI.Helpers;

namespace EmployeeManagementAPI.Services;

public class DatabaseService : IDatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException("Connection string is not configured");
    }

    public async Task<MySqlConnection> GetConnectionAsync()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return connection;
    }

    public async Task<int> ExecuteNonQueryAsync(string query, params MySqlParameter[] parameters)
    {
        await using var connection = await GetConnectionAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddRange(parameters);
        return await command.ExecuteNonQueryAsync();
    }

    public async Task<object?> ExecuteScalarAsync(string query, params MySqlParameter[] parameters)
    {
        await using var connection = await GetConnectionAsync();
        await using var command = new MySqlCommand(query, connection);
        command.Parameters.AddRange(parameters);
        return await command.ExecuteScalarAsync();
    }

    public async Task<MySqlDataReader> ExecuteReaderAsync(string query, params MySqlParameter[] parameters)
    {
        var connection = await GetConnectionAsync();
        var command = new MySqlCommand(query, connection);
        command.Parameters.AddRange(parameters);
        return await command.ExecuteReaderAsync();
    }
}