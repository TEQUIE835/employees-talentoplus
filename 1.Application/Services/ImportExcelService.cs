using _1.Application.DTOs.Employees;
using _1.Application.Interfaces.AuthInterfaces;
using _1.Application.Interfaces.DepartmentInterfaces;
using _1.Application.Interfaces.EducationalLevelInterfaces;
using _1.Application.Interfaces.EmailInterfaces;
using _1.Application.Interfaces.EmployeeInterfaces;
using _2.Domain.Entities;
using _2.Domain.ValueObjects;

namespace _1.Application.Services;

public class ImportExcelService
{
     private readonly IEmployeeRepository _employeeRepo;
    private readonly IDepartmentRepository _departmentRepo;
    private readonly IEducationalLevelRepository _levelRepo;
    private readonly IEmailSender _emailSender;
    private readonly IPasswordHasher _hasherService;
    private readonly IEmployeeDepartmentRepository _employeeDepartmentRepo;
    public ImportExcelService(
        IEmployeeRepository employeeRepo,
        IDepartmentRepository departmentRepo,
        IEducationalLevelRepository levelRepo,
        IEmailSender emailSender, IPasswordHasher hasherService,
        IEmployeeDepartmentRepository employeeDepartmentRepo)
    {
        _employeeRepo = employeeRepo;
        _departmentRepo = departmentRepo;
        _levelRepo = levelRepo;
        _emailSender = emailSender;
        _hasherService = hasherService;
        _employeeDepartmentRepo = employeeDepartmentRepo;
    }

    public async Task ImportEmployeesAsync(List<EmployeeImportDto> employees)
    {
        foreach (var dto in employees)
        {
            var departament = await _departmentRepo.GetByNameAsync(dto.Departamento)
                              ?? await _departmentRepo.AddAsync(new Department { Name = dto.Departamento });
            
            var nivel = await _levelRepo.GetByDescriptionAsync(dto.NivelEducativo)
                        ?? await _levelRepo.AddAsync(new EducationalLevel(){ Description = dto.NivelEducativo });
            
            if (await _employeeRepo.ExistsByEmailAsync(dto.Email) || await _employeeRepo.ExistsByDocumentAsync(dto.Documento))
                continue;

            
            var password = GenerarPassword();
            var passwordHash = _hasherService.Hash(password);
            var employee = new Employee
            {
                Document = dto.Documento,
                FirstName = dto.Nombres,
                LastName = dto.Apellidos,
                DateOfBirth = dto.FechaNacimiento,
                Address = dto.Direccion,
                PhoneNumber = dto.Telefono,
                Email = Email.Create(dto.Email),
                EntryDate = dto.FechaIngreso,
                EducationalLevelId = nivel.Id,
                PasswordHash = passwordHash,
            };

            employee = await _employeeRepo.AddAsync(employee);
            
            
            var statusMap = new Dictionary<string, Status>
            {
                ["Activo"] = Status.Active,
                ["Inactivo"] = Status.Inactive,
                ["Suspendido"] = Status.Suspended,
                ["Vacaciones"] = Status.OnVacation
            };
            var employeeDepartment = new EmployeeDepartment
            {
                EmployeeId = employee.Id,
                DepartmentId = departament.Id,
                Position = dto.Cargo,
                Salary = dto.Salario,
                EmployeeStatus = statusMap[dto.Estado],
            };
            var template = "<!DOCTYPE html>\n<html lang=\"es\">\n<head>\n<meta charset=\"UTF-8\">\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n<title>Bienvenido</title>\n<style>\n    body {{\n        font-family: Arial, sans-serif;\n        background-color: #f4f4f7;\n        margin: 0;\n        padding: 0;\n    }}\n    .container {{\n        max-width: 600px;\n        margin: 40px auto;\n        background-color: #ffffff;\n        padding: 30px;\n        border-radius: 8px;\n        box-shadow: 0 4px 10px rgba(0,0,0,0.1);\n    }}\n    h1 {{\n        color: #333333;\n        text-align: center;\n    }}\n    p {{\n        color: #555555;\n        line-height: 1.5;\n    }}\n    .credentials {{\n        background-color: #f0f8ff;\n        border: 1px solid #d1e7ff;\n        padding: 15px;\n        border-radius: 6px;\n        margin: 20px 0;\n        font-family: monospace;\n    }}\n    .btn {{\n        display: inline-block;\n        background-color: #007bff;\n        color: white !important;\n        text-decoration: none;\n        padding: 12px 20px;\n        border-radius: 6px;\n        margin-top: 10px;\n        font-weight: bold;\n    }}\n    .footer {{\n        font-size: 12px;\n        color: #999999;\n        text-align: center;\n        margin-top: 30px;\n    }}\n</style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>¡Bienvenido a Talento Plus!</h1>\n        <p>Hola <strong>{NombreEmpleado}</strong>,</p>\n        <p>Tu cuenta ha sido creada exitosamente. A continuación encontrarás tus credenciales de acceso:</p>\n\n        <div class=\"credentials\">\n            Usuario: <strong>{correo}</strong><br>\n            Contraseña: <strong>{contraseña}</strong>\n        </div>\n\n       <p>Te recomendamos cambiar tu contraseña la primera vez que ingreses.</p>\n\n        <div class=\"footer\">\n            © [Año] [Nombre de la Empresa]. Todos los derechos reservados.\n        </div>\n    </div>\n</body>\n</html>\n";
            
            string emailBody = template.Replace("{NombreEmpleado}", $"{employee.FirstName} {employee.LastName}");
            emailBody = emailBody.Replace("{correo}", employee.Email.ToString());
            emailBody = emailBody.Replace("{contraseña}", password);
            
            await _emailSender.SendAsync(employee.Email.ToString(), "Registro de Talento Plus", emailBody);
            await _employeeDepartmentRepo.AddAsync(employeeDepartment);
        }
    }

    private string GenerarPassword()
    {
        return Path.GetRandomFileName().Replace(".", "").Substring(0, 8); // ejemplo simple
    }
}