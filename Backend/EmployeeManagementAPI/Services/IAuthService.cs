using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Services;

public interface IAuthService
{
    Task<UserResponseDto?> RegisterAsync(UserRegisterDto registerDto);
    Task<LoginResponseDto> LoginAsync(UserLoginDto loginDto);
    Task<UserResponseDto?> GetUserProfileAsync(int userId);
    string GenerateJwtToken(User user);
    bool VerifyPassword(string password, string passwordHash);
    string HashPassword(string password);
}