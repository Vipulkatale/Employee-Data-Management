using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync();
    Task<EmployeeResponseDto?> GetEmployeeByIdAsync(int id);
    Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeCreateDto employeeDto, int createdBy);
    Task<EmployeeResponseDto?> UpdateEmployeeAsync(int id, EmployeeUpdateDto employeeDto);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<IEnumerable<EmployeeResponseDto>> SearchEmployeesAsync(string searchTerm);
    Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(string department);
    Task<IEnumerable<string>> GetDepartmentsAsync();
    Task<EmployeeStatisticsDto> GetEmployeeStatisticsAsync();
}