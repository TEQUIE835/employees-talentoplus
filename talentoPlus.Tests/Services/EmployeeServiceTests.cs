using _1.Application.DTOs.Employees;
using _1.Application.Interfaces.AuthInterfaces;
using _1.Application.Interfaces.EmailInterfaces;
using _1.Application.Interfaces.EmployeeInterfaces;
using _1.Application.Services;
using _2.Domain.Entities;

namespace talentoPlus.Tests.Services;
using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Threading.Tasks;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepoMock = new();
    private readonly Mock<IEmployeeDepartmentRepository> _employeeDeptRepoMock = new();
    private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
    private readonly Mock<IEmailSender> _emailSenderMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly Mock<IRefreshTokenRepository> _refreshTokenRepoMock = new();
    private readonly EmployeeServices _service;

    public EmployeeServiceTests()
    {
        _service = new EmployeeServices(
            _employeeRepoMock.Object,
            _employeeDeptRepoMock.Object,
            _passwordHasherMock.Object,
            _emailSenderMock.Object,
            _jwtServiceMock.Object,
            _refreshTokenRepoMock.Object
        );
    }

    [Fact]
    public async Task RegisterEmployeeAsync_ShouldThrow_WhenEmailExists()
    {
        // Arrange
        var dto = new EmployeeRegisterDto { Email = "test@example.com", Document = "123" };
        _employeeRepoMock.Setup(x => x.ExistsByEmailAsync(dto.Email)).ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _service.RegisterEmployeeAsync(dto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("El email ya está en uso.");
    }

    [Fact]
    public async Task RegisterEmployeeAsync_ShouldThrow_WhenDocumentExists()
    {
        var dto = new EmployeeRegisterDto { Email = "test@example.com", Document = "123" };
        _employeeRepoMock.Setup(x => x.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
        _employeeRepoMock.Setup(x => x.ExistsByDocumentAsync(dto.Document)).ReturnsAsync(true);

        Func<Task> act = async () => await _service.RegisterEmployeeAsync(dto);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("El documento ya está en uso.");
    }

    [Fact]
    public async Task RegisterEmployeeAsync_ShouldRegisterEmployee_WhenDataIsValid()
    {
        var dto = new EmployeeRegisterDto
        {
            Email = "test@example.com",
            Document = "123",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123",
            DateOfBirth = DateTime.Parse("1990-01-01"),
            Address = "Address",
            PhoneNumber = "123456789",
            EntryDate = DateTime.Now,
            EducationalLevelId = 1,
            ProfessionalProfile = "Profile",
            DepartmentId = 2,
            Salary = 1000,
            Position = "Developer"
        };

        _employeeRepoMock.Setup(x => x.ExistsByEmailAsync(dto.Email)).ReturnsAsync(false);
        _employeeRepoMock.Setup(x => x.ExistsByDocumentAsync(dto.Document)).ReturnsAsync(false);
        _passwordHasherMock.Setup(x => x.Hash(dto.Password)).Returns("hashedPassword");
        _employeeRepoMock.Setup(x => x.AddAsync(It.IsAny<Employee>())).ReturnsAsync((Employee e) =>
        {
            e.Id = 1; // simula asignación de ID
            return e;
        });
        _employeeDeptRepoMock.Setup(x => x.AddAsync(It.IsAny<EmployeeDepartment>())).Returns(Task.CompletedTask);
        _emailSenderMock.Setup(x => x.SendAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

        // Act
        Func<Task> act = async () => await _service.RegisterEmployeeAsync(dto);

        // Assert
        await act.Should().NotThrowAsync();
        _employeeRepoMock.Verify(x => x.AddAsync(It.IsAny<Employee>()), Times.Once);
        _employeeDeptRepoMock.Verify(x => x.AddAsync(It.IsAny<EmployeeDepartment>()), Times.Once);
        _emailSenderMock.Verify(x => x.SendAsync(dto.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }
}
