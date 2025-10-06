using MySqlConnector;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;

namespace EmployeeManagementAPI.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDatabaseService _databaseService;

    public EmployeeRepository(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        const string query = @"
            SELECT Id, EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                   Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, PostalCode, 
                   ProfilePicture, IsActive, CreatedBy, CreatedAt, UpdatedAt
            FROM Employees 
            WHERE IsActive = TRUE
            ORDER BY CreatedAt DESC";

        var employees = new List<Employee>();

        await using var reader = await _databaseService.ExecuteReaderAsync(query);

        while (await reader.ReadAsync())
        {
            employees.Add(MapEmployeeFromReader(reader));
        }

        return employees;
    }

    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        const string query = @"
            SELECT Id, EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                   Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, PostalCode, 
                   ProfilePicture, IsActive, CreatedBy, CreatedAt, UpdatedAt
            FROM Employees 
            WHERE Id = @Id AND IsActive = TRUE";

        var parameters = new[] { new MySqlParameter("@Id", id) };

        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        if (await reader.ReadAsync())
        {
            return MapEmployeeFromReader(reader);
        }

        return null;
    }

    public async Task<Employee?> GetEmployeeByEmailAsync(string email)
    {
        const string query = @"
            SELECT Id, EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                   Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, PostalCode, 
                   ProfilePicture, IsActive, CreatedBy, CreatedAt, UpdatedAt
            FROM Employees 
            WHERE Email = @Email AND IsActive = TRUE";

        var parameters = new[] { new MySqlParameter("@Email", email) };

        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        if (await reader.ReadAsync())
        {
            return MapEmployeeFromReader(reader);
        }

        return null;
    }

    public async Task<Employee?> GetEmployeeByCodeAsync(string employeeCode)
    {
        const string query = @"
            SELECT Id, EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                   Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, PostalCode, 
                   ProfilePicture, IsActive, CreatedBy, CreatedAt, UpdatedAt
            FROM Employees 
            WHERE EmployeeCode = @EmployeeCode AND IsActive = TRUE";

        var parameters = new[] { new MySqlParameter("@EmployeeCode", employeeCode) };

        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        if (await reader.ReadAsync())
        {
            return MapEmployeeFromReader(reader);
        }

        return null;
    }

    public async Task<int> CreateEmployeeAsync(Employee employee)
    {
        // Generate employee code
        employee.EmployeeCode = await GenerateEmployeeCodeAsync();

        const string query = @"
            INSERT INTO Employees (
                EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, 
                PostalCode, ProfilePicture, CreatedBy
            ) VALUES (
                @EmployeeCode, @FirstName, @LastName, @Email, @Phone, @Position, @Department,
                @Salary, @DateOfBirth, @DateOfJoining, @Address, @City, @State, @Country,
                @PostalCode, @ProfilePicture, @CreatedBy
            );
            SELECT LAST_INSERT_ID();";

        var parameters = new[]
        {
            new MySqlParameter("@EmployeeCode", employee.EmployeeCode),
            new MySqlParameter("@FirstName", employee.FirstName),
            new MySqlParameter("@LastName", employee.LastName),
            new MySqlParameter("@Email", employee.Email),
            new MySqlParameter("@Phone", employee.Phone ?? (object)DBNull.Value),
            new MySqlParameter("@Position", employee.Position),
            new MySqlParameter("@Department", employee.Department),
            new MySqlParameter("@Salary", employee.Salary ?? (object)DBNull.Value),
            new MySqlParameter("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value),
            new MySqlParameter("@DateOfJoining", employee.DateOfJoining),
            new MySqlParameter("@Address", employee.Address ?? (object)DBNull.Value),
            new MySqlParameter("@City", employee.City ?? (object)DBNull.Value),
            new MySqlParameter("@State", employee.State ?? (object)DBNull.Value),
            new MySqlParameter("@Country", employee.Country),
            new MySqlParameter("@PostalCode", employee.PostalCode ?? (object)DBNull.Value),
            new MySqlParameter("@ProfilePicture", employee.ProfilePicture ?? (object)DBNull.Value),
            new MySqlParameter("@CreatedBy", employee.CreatedBy ?? (object)DBNull.Value)
        };

        var result = await _databaseService.ExecuteScalarAsync(query, parameters);
        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> UpdateEmployeeAsync(Employee employee)
    {
        const string query = @"
            UPDATE Employees 
            SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone, 
                Position = @Position, Department = @Department, Salary = @Salary,
                DateOfBirth = @DateOfBirth, Address = @Address, City = @City,
                State = @State, Country = @Country, PostalCode = @PostalCode,
                ProfilePicture = @ProfilePicture, IsActive = @IsActive,
                UpdatedAt = CURRENT_TIMESTAMP
            WHERE Id = @Id";

        var parameters = new[]
        {
            new MySqlParameter("@Id", employee.Id),
            new MySqlParameter("@FirstName", employee.FirstName),
            new MySqlParameter("@LastName", employee.LastName),
            new MySqlParameter("@Phone", employee.Phone ?? (object)DBNull.Value),
            new MySqlParameter("@Position", employee.Position),
            new MySqlParameter("@Department", employee.Department),
            new MySqlParameter("@Salary", employee.Salary ?? (object)DBNull.Value),
            new MySqlParameter("@DateOfBirth", employee.DateOfBirth ?? (object)DBNull.Value),
            new MySqlParameter("@Address", employee.Address ?? (object)DBNull.Value),
            new MySqlParameter("@City", employee.City ?? (object)DBNull.Value),
            new MySqlParameter("@State", employee.State ?? (object)DBNull.Value),
            new MySqlParameter("@Country", employee.Country),
            new MySqlParameter("@PostalCode", employee.PostalCode ?? (object)DBNull.Value),
            new MySqlParameter("@ProfilePicture", employee.ProfilePicture ?? (object)DBNull.Value),
            new MySqlParameter("@IsActive", employee.IsActive)
        };

        var affectedRows = await _databaseService.ExecuteNonQueryAsync(query, parameters);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        const string query = "UPDATE Employees SET IsActive = FALSE WHERE Id = @Id";
        var parameters = new[] { new MySqlParameter("@Id", id) };

        var affectedRows = await _databaseService.ExecuteNonQueryAsync(query, parameters);
        return affectedRows > 0;
    }

    public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
    {
        string query = "SELECT COUNT(1) FROM Employees WHERE Email = @Email AND IsActive = TRUE";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@Email", email)
        };

        if (excludeId.HasValue)
        {
            query += " AND Id != @ExcludeId";
            parameters.Add(new MySqlParameter("@ExcludeId", excludeId.Value));
        }

        var result = await _databaseService.ExecuteScalarAsync(query, parameters.ToArray());
        return result != null && Convert.ToInt32(result) > 0;
    }

    public async Task<bool> EmployeeCodeExistsAsync(string employeeCode, int? excludeId = null)
    {
        string query = "SELECT COUNT(1) FROM Employees WHERE EmployeeCode = @EmployeeCode AND IsActive = TRUE";
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("@EmployeeCode", employeeCode)
        };

        if (excludeId.HasValue)
        {
            query += " AND Id != @ExcludeId";
            parameters.Add(new MySqlParameter("@ExcludeId", excludeId.Value));
        }

        var result = await _databaseService.ExecuteScalarAsync(query, parameters.ToArray());
        return result != null && Convert.ToInt32(result) > 0;
    }

    public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
    {
        const string query = @"
            SELECT Id, EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                   Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, PostalCode, 
                   ProfilePicture, IsActive, CreatedBy, CreatedAt, UpdatedAt
            FROM Employees 
            WHERE IsActive = TRUE AND (
                FirstName LIKE @SearchTerm OR 
                LastName LIKE @SearchTerm OR 
                Email LIKE @SearchTerm OR 
                EmployeeCode LIKE @SearchTerm OR
                Position LIKE @SearchTerm OR
                Department LIKE @SearchTerm
            )
            ORDER BY CreatedAt DESC";

        var parameters = new[] { new MySqlParameter("@SearchTerm", $"%{searchTerm}%") };

        var employees = new List<Employee>();
        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        while (await reader.ReadAsync())
        {
            employees.Add(MapEmployeeFromReader(reader));
        }

        return employees;
    }

    public async Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department)
    {
        const string query = @"
            SELECT Id, EmployeeCode, FirstName, LastName, Email, Phone, Position, Department, 
                   Salary, DateOfBirth, DateOfJoining, Address, City, State, Country, PostalCode, 
                   ProfilePicture, IsActive, CreatedBy, CreatedAt, UpdatedAt
            FROM Employees 
            WHERE Department = @Department AND IsActive = TRUE
            ORDER BY FirstName, LastName";

        var parameters = new[] { new MySqlParameter("@Department", department) };

        var employees = new List<Employee>();
        await using var reader = await _databaseService.ExecuteReaderAsync(query, parameters);

        while (await reader.ReadAsync())
        {
            employees.Add(MapEmployeeFromReader(reader));
        }

        return employees;
    }

    private async Task<string> GenerateEmployeeCodeAsync()
    {
        const string query = "SELECT COUNT(1) FROM Employees";
        var result = await _databaseService.ExecuteScalarAsync(query);
        var count = result != null ? Convert.ToInt32(result) + 1 : 1;

        return $"EMP{count.ToString().PadLeft(5, '0')}";
    }

    private Employee MapEmployeeFromReader(MySqlDataReader reader)
    {
        return new Employee
        {
            Id = reader.GetInt32("Id"),
            EmployeeCode = reader.GetString("EmployeeCode"),
            FirstName = reader.GetString("FirstName"),
            LastName = reader.GetString("LastName"),
            Email = reader.GetString("Email"),
            Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString("Phone"),
            Position = reader.GetString("Position"),
            Department = reader.GetString("Department"),
            Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? null : reader.GetDecimal("Salary"),
            DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime("DateOfBirth"),
            DateOfJoining = reader.GetDateTime("DateOfJoining"),
            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString("Address"),
            City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString("City"),
            State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString("State"),
            Country = reader.GetString("Country"),
            PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : reader.GetString("PostalCode"),
            ProfilePicture = reader.IsDBNull(reader.GetOrdinal("ProfilePicture")) ? null : reader.GetString("ProfilePicture"),
            IsActive = reader.GetBoolean("IsActive"),
            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetInt32("CreatedBy"),
            CreatedAt = reader.GetDateTime("CreatedAt"),
            UpdatedAt = reader.GetDateTime("UpdatedAt")
        };
    }
}