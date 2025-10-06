using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;

namespace EmployeeManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployees()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeResponseDto>> GetEmployee(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
        {
            return NotFound($"Employee with ID {id} not found");
        }

        return Ok(employee);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EmployeeResponseDto>> CreateEmployee(EmployeeCreateDto employeeDto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var employee = await _employeeService.CreateEmployeeAsync(employeeDto, userId);
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EmployeeResponseDto>> UpdateEmployee(int id, EmployeeUpdateDto employeeDto)
    {
        try
        {
            var employee = await _employeeService.UpdateEmployeeAsync(id, employeeDto);
            if (employee == null)
            {
                return NotFound($"Employee with ID {id} not found");
            }

            return Ok(employee);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        var success = await _employeeService.DeleteEmployeeAsync(id);
        if (!success)
        {
            return NotFound($"Employee with ID {id} not found");
        }

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> SearchEmployees([FromQuery] string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return BadRequest("Search term is required");
        }

        var employees = await _employeeService.SearchEmployeesAsync(searchTerm);
        return Ok(employees);
    }

    [HttpGet("department/{department}")]
    public async Task<ActionResult<IEnumerable<EmployeeResponseDto>>> GetEmployeesByDepartment(string department)
    {
        var employees = await _employeeService.GetEmployeesByDepartmentAsync(department);
        return Ok(employees);
    }

    [HttpGet("departments")]
    public async Task<ActionResult<IEnumerable<string>>> GetDepartments()
    {
        var departments = await _employeeService.GetDepartmentsAsync();
        return Ok(departments);
    }

    [HttpGet("statistics")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EmployeeStatisticsDto>> GetStatistics()
    {
        var statistics = await _employeeService.GetEmployeeStatisticsAsync();
        return Ok(statistics);
    }
}