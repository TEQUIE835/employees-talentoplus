using _1.Application.DTOs.Employees;
using _2.Domain.Entities;
using _2.Domain.ValueObjects;
using _1.Application.Interfaces.AuthInterfaces;
using _1.Application.Interfaces.EmailInterfaces;
using _1.Application.Interfaces.EmployeeInterfaces;
using _2.Domain.ValueObjects;

namespace _1.Application.Services;

public class EmployeeServices
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeDepartmentRepository _employeeDepartmentRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailSender _emailSender;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    public EmployeeServices(
        IEmployeeRepository employeeRepository,
        IEmployeeDepartmentRepository employeeDepartmentRepository,
        IPasswordHasher passwordHasher,
        IEmailSender emailSender,
        IJwtService jwtService,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _employeeRepository = employeeRepository;
        _employeeDepartmentRepository = employeeDepartmentRepository;
        _passwordHasher = passwordHasher;
        _emailSender = emailSender;
        _jwtService = jwtService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task RegisterEmployeeAsync(EmployeeRegisterDto newEmployeeDto)
    {
        Console.WriteLine("Consolaso-1");
        var existingByEmail = await _employeeRepository.ExistsByEmailAsync(newEmployeeDto.Email);
        Console.WriteLine("consolaso");
        if (existingByEmail)
            throw new ArgumentException("El email ya está en uso.");
        var existingByDocument = await _employeeRepository.ExistsByDocumentAsync(newEmployeeDto.Document);
        Console.WriteLine("Consolaso2");
        if (existingByDocument) 
            throw new ArgumentException("El documento ya está en uso.");
        var hashedPassword = _passwordHasher.Hash(newEmployeeDto.Password);
        Console.WriteLine("Consolaso3");
        var employee = new Employee
        {
            FirstName = newEmployeeDto.FirstName,
            LastName = newEmployeeDto.LastName,
            Document = newEmployeeDto.Document,
            Email = Email.Create(newEmployeeDto.Email),
            DateOfBirth = newEmployeeDto.DateOfBirth,
            PasswordHash = hashedPassword,
            Address = newEmployeeDto.Address,
            PhoneNumber = newEmployeeDto.PhoneNumber,
            EntryDate = newEmployeeDto.EntryDate,
            EducationalLevelId = newEmployeeDto.EducationalLevelId,
            ProfessionalProfile = newEmployeeDto.ProfessionalProfile,
        };
        Console.WriteLine(employee.Email);
        employee = await _employeeRepository.AddAsync(employee);
        Console.WriteLine("ConsolasoID" + employee.Id);
        var employeeDepartment = new EmployeeDepartment
        {
            EmployeeId = employee.Id,
            DepartmentId = newEmployeeDto.DepartmentId,
            Salary = newEmployeeDto.Salary,
            Position = newEmployeeDto.Position,
            EmployeeStatus = Status.Active
        };
        Console.WriteLine("consolasodepartment" + employee.Email.Value);
        await _employeeDepartmentRepository.AddAsync(employeeDepartment);
        string template =
            "<!DOCTYPE html>\n<html lang=\"es\">\n<head>\n    <meta charset=\"UTF-8\">\n    <title>Bienvenido a Nuestra Plataforma</title>\n    <style>\n        body {{ font-family: Arial, sans-serif; background-color: #f5f5f5; margin: 0; padding: 0; }}\n        .container {{ max-width: 600px; background-color: #fff; margin: 40px auto; padding: 30px; border-radius: 8px; box-shadow: 0px 4px 10px rgba(0,0,0,0.1); }}\n        h1 {{ color: #333; text-align: center; }}\n        p {{ font-size: 16px; color: #555; line-height: 1.5; }}\n        .footer {{ font-size: 12px; color: #aaa; text-align: center; margin-top: 30px; }}\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>¡Bienvenido a Nuestra Plataforma!</h1>\n        <p>Hola <strong>{NombreEmpleado}</strong>,</p>\n        <p>Tu cuenta ha sido creada exitosamente. Ya puedes iniciar sesión y empezar a explorar nuestra plataforma.</p>\n        <p>¡Nos alegra tenerte con nosotros!</p>\n        <div class=\"footer\">\n            &copy; 2025 Nuestra Plataforma. Todos los derechos reservados.\n        </div>\n    </div>\n</body>\n</html>\n";
        string emailBody = template.Replace("{NombreEmpleado}", $"{employee.FirstName} {employee.LastName}");
        
        await _emailSender.SendAsync(employee.Email.Value, "Welcome to the Company", emailBody);
    }
    
    public async Task<LoginResultDto> LoginAsync(EmployeeLoginDto loginDto)
    {
        var employee = await _employeeRepository.GetByEmailAsync(loginDto.Email);
        if (employee == null)
            throw new ArgumentException("Credenciales inválidas.");
        var isPasswordValid = _passwordHasher.Verify(loginDto.Password, employee.PasswordHash);
        if (!isPasswordValid)
            throw new ArgumentException("Credenciales inválidas.");
        var accessToken = _jwtService.GenerateAccessToken(employee);
        var refreshToken = _jwtService.GenerateRefreshToken();
        await _refreshTokenRepository.AddAsync(
            new RefreshToken()
            {
                EmployeeId = employee.Id,
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7)
            });

        return new LoginResultDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync();
        return employees;
    }
    
    public async Task<LoginResultDto> UpdateAsync(EmployeeUpdateDto updatedEmployee)
    {
        var employeeByDocument = await _employeeRepository.GetByDocumentAsync(updatedEmployee.Document);
        if (employeeByDocument != null && employeeByDocument.Id != updatedEmployee.Id)
            throw new ArgumentException("El documento ya está en uso.");
        var employeeByEmail = await _employeeRepository.GetByEmailAsync(updatedEmployee.Email);
        if (employeeByEmail != null && employeeByEmail.Id != updatedEmployee.Id)
            throw new ArgumentException("El email ya está en uso.");
        var employee = await _employeeRepository.GetByIdAsync(updatedEmployee.Id);
        if (employee == null)
            throw new ArgumentException("Empleado no encontrado.");
        employee.FirstName = updatedEmployee.FirstName;
        employee.LastName = updatedEmployee.LastName;
        employee.Document = updatedEmployee.Document;
        employee.Email = Email.Create(updatedEmployee.Email);
        employee.DateOfBirth = updatedEmployee.DateOfBirth;
        employee.Address = updatedEmployee.Address;
        employee.PhoneNumber = updatedEmployee.PhoneNumber;
        employee.EducationalLevelId = updatedEmployee.EducationalLevelId;
        employee.ProfessionalProfile = updatedEmployee.ProfessionalProfile;
        await _employeeRepository.UpdateAsync(employee);
        var newAccessToken = _jwtService.GenerateAccessToken(employee);
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        await _refreshTokenRepository.RevokeAllForEmployeeAsync(employee.Id);
        await _refreshTokenRepository.AddAsync(new RefreshToken
        {
            EmployeeId = employee.Id,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(7) 
        });
        var employeeDepartment = await _employeeDepartmentRepository.GetByEmployeeIdAsync(updatedEmployee.Id);
        if (employeeDepartment == null)
            throw new ArgumentException("Empleado no asignado a ningún departamento.");
        foreach (var department in employeeDepartment)
        {
            if (department.EmployeeStatus != Status.Inactive)
            {
                department.EmployeeStatus = Status.Inactive;
                await _employeeDepartmentRepository.UpdateAsync(department);
            }
        }

        await _employeeDepartmentRepository.AddAsync(new EmployeeDepartment
        {
            EmployeeId = updatedEmployee.Id,
            DepartmentId = updatedEmployee.DepartmentId,
            Salary = updatedEmployee.Salary,
            Position = updatedEmployee.Position,
            EmployeeStatus = Status.Active
        });

        return new LoginResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    
    public async Task<Employee?> GetByIdAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        return employee;
    }

    public async Task<Employee?> GetByEmailAsync(string email)
    {
        var employee = await _employeeRepository.GetByEmailAsync(email);
        return employee;
    }
    
    public async Task DeleteAsync(int id)
    {
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee == null)
            throw new ArgumentException("Empleado no encontrado.");
        await _refreshTokenRepository.RevokeAllForEmployeeAsync(employee.Id);
        await _employeeRepository.DeleteAsync(employee);
    }
    
    public async Task<Employee?> GetByDocumentAsync(string document)
    {
        var employee = await _employeeRepository.GetByDocumentAsync(document);
        return employee;
    }
    
    public async Task<LoginResultDto> RefreshTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (token == null || token.Expires < DateTime.UtcNow)
            throw new ArgumentException("Refresh token inválido o expirado.");

        var employee = await _employeeRepository.GetByIdAsync(token.EmployeeId);
    
        var newAccessToken = _jwtService.GenerateAccessToken(employee);
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        
        await _refreshTokenRepository.AddAsync(new RefreshToken
        {
            EmployeeId = employee.Id,
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(7) 
        });
        await _refreshTokenRepository.RevokeTokenAsync(token);

        return new LoginResultDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
    
    public async Task RevokeTokensByEmployeeAsync(int employeeId)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null)
            throw new ArgumentException("Empleado no encontrado.");
        await _refreshTokenRepository.RevokeAllForEmployeeAsync(employeeId);
    }
    
    public async Task RevokeTokenAsync(string refreshToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (token == null)
            throw new ArgumentException("Refresh token inválido.");
        await _refreshTokenRepository.RevokeTokenAsync(token);
    }

}