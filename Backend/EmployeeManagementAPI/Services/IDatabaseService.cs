using MySqlConnector;

namespace EmployeeManagementAPI.Services;

public interface IDatabaseService
{
    Task<MySqlConnection> GetConnectionAsync();
    Task<int> ExecuteNonQueryAsync(string query, params MySqlParameter[] parameters);
    Task<object?> ExecuteScalarAsync(string query, params MySqlParameter[] parameters);
    Task<MySqlDataReader> ExecuteReaderAsync(string query, params MySqlParameter[] parameters);
}