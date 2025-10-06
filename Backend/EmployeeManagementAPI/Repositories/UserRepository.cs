using MySqlConnector;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;

namespace EmployeeManagementAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDatabaseService _databaseService;

    public UserRepository(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        const string query = @"
            SELECT Id, Username, Email, PasswordHash, Role, CreatedAt, IsActive 
            FROM Users 
            WHERE Id = @Id AND IsActive = TRUE";

        var parameters = new[]
        {
            new MySqlParameter("@Id", id)
        };

        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32("Id"),
                Username = reader.GetString("Username"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                Role = reader.GetString("Role"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }

        return null;
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        const string query = @"
            SELECT Id, Username, Email, PasswordHash, Role, CreatedAt, IsActive 
            FROM Users 
            WHERE Username = @Username AND IsActive = TRUE";

        var parameters = new[]
        {
            new MySqlParameter("@Username", username)
        };

        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32("Id"),
                Username = reader.GetString("Username"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                Role = reader.GetString("Role"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }

        return null;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        const string query = @"
            SELECT Id, Username, Email, PasswordHash, Role, CreatedAt, IsActive 
            FROM Users 
            WHERE Email = @Email AND IsActive = TRUE";

        var parameters = new[]
        {
            new MySqlParameter("@Email", email)
        };

        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32("Id"),
                Username = reader.GetString("Username"),
                Email = reader.GetString("Email"),
                PasswordHash = reader.GetString("PasswordHash"),
                Role = reader.GetString("Role"),
                CreatedAt = reader.GetDateTime("CreatedAt"),
                IsActive = reader.GetBoolean("IsActive")
            };
        }

        return null;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        const string query = @"
            INSERT INTO Users (Username, Email, PasswordHash, Role, IsActive)
            VALUES (@Username, @Email, @PasswordHash, @Role, @IsActive);
            SELECT LAST_INSERT_ID();";

        var parameters = new[]
        {
            new MySqlParameter("@Username", user.Username),
            new MySqlParameter("@Email", user.Email),
            new MySqlParameter("@PasswordHash", user.PasswordHash),
            new MySqlParameter("@Role", user.Role),
            new MySqlParameter("@IsActive", user.IsActive)
        };

        var result = await _databaseService.ExecuteScalarAsync(query, parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        const string query = @"
            UPDATE Users 
            SET Username = @Username, Email = @Email, PasswordHash = @PasswordHash, 
                Role = @Role, IsActive = @IsActive
            WHERE Id = @Id";

        var parameters = new[]
        {
            new MySqlParameter("@Id", user.Id),
            new MySqlParameter("@Username", user.Username),
            new MySqlParameter("@Email", user.Email),
            new MySqlParameter("@PasswordHash", user.PasswordHash),
            new MySqlParameter("@Role", user.Role),
            new MySqlParameter("@IsActive", user.IsActive)
        };

        var affectedRows = await _databaseService.ExecuteNonQueryAsync(query, parameters);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        const string query = "UPDATE Users SET IsActive = FALSE WHERE Id = @Id";
        var parameters = new[] { new MySqlParameter("@Id", id) };

        var affectedRows = await _databaseService.ExecuteNonQueryAsync(query, parameters);
        return affectedRows > 0;
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        const string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND IsActive = TRUE";
        var parameters = new[] { new MySqlParameter("@Username", username) };

        var result = await _databaseService.ExecuteScalarAsync(query, parameters);
        return result != null && Convert.ToInt32(result) > 0;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        const string query = "SELECT COUNT(1) FROM Users WHERE Email = @Email AND IsActive = TRUE";
        var parameters = new[] { new MySqlParameter("@Email", email) };

        var result = await _databaseService.ExecuteScalarAsync(query, parameters);
        return result != null && Convert.ToInt32(result) > 0;
    }
}