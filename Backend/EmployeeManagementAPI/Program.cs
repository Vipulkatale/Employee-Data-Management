using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Repositories;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure Database Service
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

// Configure Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Configure Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "EmployeeManagementAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "EmployeeManagementUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure Swagger/OpenAPI with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Employee Management API",
        Version = "v1",
        Description = "A comprehensive Employee Management System with JWT Authentication"
    });

    // Add JWT Auth to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Test database connection before starting the app
try
{
    using var scope = app.Services.CreateScope();
    var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
    var connection = await databaseService.GetConnectionAsync();
    Console.WriteLine("Database connection successful!");
    await connection.CloseAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Database connection failed: {ex.Message}");
    throw;
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowAngular");

// Add Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed initial data
await SeedInitialData(app.Services);

app.Run();

async Task SeedInitialData(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

    // Check if admin user exists
    var adminUser = await userRepository.GetUserByUsernameAsync("admin");
    if (adminUser == null)
    {
        var adminDto = new UserRegisterDto
        {
            Username = "admin",
            Email = "admin@company.com",
            Password = "Admin123!",
            Role = "Admin"
        };
        await authService.RegisterAsync(adminDto);
        Console.WriteLine("Admin user created");
    }
    else
    {
        Console.WriteLine("Admin user already exists");
    }

    // Create manager user
    var managerUser = await userRepository.GetUserByUsernameAsync("manager");
    if (managerUser == null)
    {
        var managerDto = new UserRegisterDto
        {
            Username = "manager",
            Email = "manager@company.com",
            Password = "Manager123!",
            Role = "Manager"
        };
        await authService.RegisterAsync(managerDto);
        Console.WriteLine("Manager user created");
    }
    else
    {
        Console.WriteLine("Manager user already exists");
    }
}