using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Repositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<Employee?> GetEmployeeByIdAsync(int id);
    Task<Employee?> GetEmployeeByEmailAsync(string email);
    Task<Employee?> GetEmployeeByCodeAsync(string employeeCode);
    Task<int> CreateEmployeeAsync(Employee employee);
    Task<bool> UpdateEmployeeAsync(Employee employee);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    Task<bool> EmployeeCodeExistsAsync(string employeeCode, int? excludeId = null);
    Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);
    Task<IEnumerable<Employee>> GetEmployeesByDepartmentAsync(string department);
}