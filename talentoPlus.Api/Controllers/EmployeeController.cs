using System.Linq.Expressions;
using _1.Application.DTOs.Employees;
using _1.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace talentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeServices _employeeServices;
    public EmployeeController(EmployeeServices employeeServices)
    {
        _employeeServices = employeeServices;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeRegisterDto newEmployee)
    {
        try
        {
            await _employeeServices.RegisterEmployeeAsync(newEmployee);
            return Ok("Employee created successfully");
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] EmployeeLoginDto loginDto)
    {
        try
        {
            var token = await _employeeServices.LoginAsync(loginDto);
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }
    
    [HttpPost("refreshtoken")]
    public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            var newToken = await _employeeServices.RefreshTokenAsync(refreshToken);
            return Ok(newToken);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpPost("revoke/{employeeId}")]
    public async Task<IActionResult> RevokeTokens(int employeeId)
    {
        try
        {
            await _employeeServices.RevokeTokensByEmployeeAsync(employeeId);
            return Ok("Tokens revoked successfully");
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }
    
    [Authorize]
    [HttpPost("revoke")]
    public async Task<IActionResult> RevokeToken([FromBody] string refreshToken)
    {
        try
        {
            await _employeeServices.RevokeTokenAsync(refreshToken);
            return Ok("Token revoked successfully");
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetEmployees()
    {
        try
        {
            var employees = await _employeeServices.GetAllAsync();
            return Ok(employees);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        try
        {
            var employee = await _employeeServices.GetByIdAsync(id);
            return Ok(employee);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpGet("searchemail")]
    public async Task<IActionResult> SearchEmployees([FromQuery] string? email)
    {
        try
        {
            if (email == null)
                throw new Exception("Email parameter is required");
            var employee = await _employeeServices.GetByEmailAsync(email);
            return Ok(employee);
            
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }
    [Authorize]
    [HttpGet("searchdocument")]
    public async Task<IActionResult> SearchEmployeesByDocument([FromQuery] string? document)
    {
        try
        {
            if (document == null)
                throw new Exception("Email parameter is required");
            var employee = await _employeeServices.GetByDocumentAsync(document);
            return Ok(employee);
            
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdateDto updatedEmployee)
    {
        try
        {
            var tokens = await _employeeServices.UpdateAsync(updatedEmployee);
            return Ok(new {message= "Update succesful",
                newtokens = tokens});
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        try
        {
            await _employeeServices.DeleteAsync(id);
            return Ok("Employee deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

}