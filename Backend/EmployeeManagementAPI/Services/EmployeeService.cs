using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Repositories;

namespace EmployeeManagementAPI.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return employees.Select(MapToEmployeeResponseDto);
    }

    public async Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        return employee != null ? MapToEmployeeResponseDto(employee) : null;
    }

    public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateDto employeeDto, int createdBy)
    {
        // Check if email already exists
        if (await _employeeRepository.EmailExistsAsync(employeeDto.Email))
        {
            throw new ArgumentException("Employee with this email already exists");
        }

        var employee = new Employee
        {
            FirstName = employeeDto.FirstName,
            LastName = employeeDto.LastName,
            Email = employeeDto.Email,
            Phone = employeeDto.Phone,
            Position = employeeDto.Position,
            Department = employeeDto.Department,
            Salary = employeeDto.Salary,
            DateOfBirth = employeeDto.DateOfBirth,
            DateOfJoining = employeeDto.DateOfJoining ?? DateTime.UtcNow.Date, // Use Date part only
            Address = employeeDto.Address,
            City = employeeDto.City,
            State = employeeDto.State,
            Country = employeeDto.Country ?? "India",
            PostalCode = employeeDto.PostalCode,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var employeeId = await _employeeRepository.CreateEmployeeAsync(employee);
        employee.Id = employeeId;

        return MapToEmployeeResponseDto(employee);
    }

    public async Task<EmployeeResponseDto?> UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeDto)
    {
        var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (existingEmployee == null)
        {
            return null;
        }

        // Check if email already exists (excluding current employee)
        if (!string.IsNullOrEmpty(employeeDto.Email) &&
            await _employeeRepository.EmailExistsAsync(employeeDto.Email, id))
        {
            throw new ArgumentException("Employee with this email already exists");
        }

        var oldValues = System.Text.Json.JsonSerializer.Serialize(existingEmployee);

        // Update properties if provided
        if (!string.IsNullOrEmpty(employeeDto.FirstName))
            existingEmployee.FirstName = employeeDto.FirstName;

        if (!string.IsNullOrEmpty(employeeDto.LastName))
            existingEmployee.LastName = employeeDto.LastName;

        if (!string.IsNullOrEmpty(employeeDto.Phone))
            existingEmployee.Phone = employeeDto.Phone;

        if (!string.IsNullOrEmpty(employeeDto.Position))
            existingEmployee.Position = employeeDto.Position;

        if (!string.IsNullOrEmpty(employeeDto.Department))
            existingEmployee.Department = employeeDto.Department;

        if (employeeDto.Salary.HasValue)
            existingEmployee.Salary = employeeDto.Salary;

        if (employeeDto.DateOfBirth.HasValue)
            existingEmployee.DateOfBirth = employeeDto.DateOfBirth;

        if (!string.IsNullOrEmpty(employeeDto.Address))
            existingEmployee.Address = employeeDto.Address;

        if (!string.IsNullOrEmpty(employeeDto.City))
            existingEmployee.City = employeeDto.City;

        if (!string.IsNullOrEmpty(employeeDto.State))
            existingEmployee.State = employeeDto.State;

        if (!string.IsNullOrEmpty(employeeDto.Country))
            existingEmployee.Country = employeeDto.Country;

        if (!string.IsNullOrEmpty(employeeDto.PostalCode))
            existingEmployee.PostalCode = employeeDto.PostalCode;

        if (employeeDto.IsActive.HasValue)
            existingEmployee.IsActive = employeeDto.IsActive.Value;

        existingEmployee.UpdatedAt = DateTime.UtcNow;

        var success = await _employeeRepository.UpdateEmployeeAsync(existingEmployee);
        if (!success)
        {
            return null;
        }

        return MapToEmployeeResponseDto(existingEmployee);
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return false;
        }

        var success = await _employeeRepository.DeleteEmployeeAsync(id);


        return success;
    }

    public async Task<IEnumerable<EmployeeResponseDto>> SearchEmployeesAsync(string searchTerm)
    {
        var employees = await _employeeRepository.SearchEmployeesAsync(searchTerm);
        return employees.Select(MapToEmployeeResponseDto);
    }

    public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(string department)
    {
        var employees = await _employeeRepository.GetEmployeesByDepartmentAsync(department);
        return employees.Select(MapToEmployeeResponseDto);
    }

    public async Task<IEnumerable<string>> GetDepartmentsAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return employees.Select(e => e.Department).Distinct().OrderBy(d => d);
    }

    public async Task<EmployeeStatisticsDto> GetEmployeeStatisticsAsync()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        var employeeList = employees.ToList();

        return new EmployeeStatisticsDto
        {
            TotalEmployees = employeeList.Count,
            ActiveEmployees = employeeList.Count(e => e.IsActive),
            DepartmentsCount = employeeList.Select(e => e.Department).Distinct().Count(),
            AverageSalary = employeeList.Where(e => e.Salary.HasValue).Average(e => e.Salary) ?? 0,
            DepartmentStats = employeeList
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentStatDto
                {
                    Department = g.Key,
                    EmployeeCount = g.Count(),
                    AverageSalary = g.Where(e => e.Salary.HasValue).Average(e => e.Salary) ?? 0
                })
                .OrderByDescending(d => d.EmployeeCount)
                .ToList()
        };
    }

    private EmployeeResponseDto MapToEmployeeResponseDto(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            EmployeeCode = employee.EmployeeCode,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Phone = employee.Phone,
            Position = employee.Position,
            Department = employee.Department,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            DateOfJoining = employee.DateOfJoining,
            Address = employee.Address,
            City = employee.City,
            State = employee.State,
            Country = employee.Country,
            PostalCode = employee.PostalCode,
            ProfilePicture = employee.ProfilePicture,
            IsActive = employee.IsActive,
            CreatedBy = employee.CreatedBy,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}

public class EmployeeStatisticsDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int DepartmentsCount { get; set; }
    public decimal AverageSalary { get; set; }
    public List<DepartmentStatDto> DepartmentStats { get; set; } = new();
}

public class DepartmentStatDto
{
    public string Department { get; set; } = string.Empty;
    public int EmployeeCount { get; set; }
    public decimal AverageSalary { get; set; }
}