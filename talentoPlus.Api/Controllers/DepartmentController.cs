using _1.Application.Services;
using _2.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace talentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly DepartmentService _departmentService;

    public DepartmentController(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDepartments()
    {
        try
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(departments);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDepartmentById(int id)
    {
        try
        {
            var department = await _departmentService.GetByIdAsync(id);
            return Ok(department);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpGet("by-name/{name}")]
    public async Task<IActionResult> GetDepartmentByName(string name)
    {
        try
        {
            var department = await _departmentService.GetByNameAsync(name);
            return Ok(department);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateDepartment([FromBody] Department department)
    {
        try
        {
            await _departmentService.CreateAsync(department);
            return Ok("Department created successfully");
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

}