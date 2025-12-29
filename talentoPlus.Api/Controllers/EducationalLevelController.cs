using _1.Application.Services;
using _2.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace talentoPlus.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EducationalLevelController : ControllerBase
{
    private readonly EducationalLevelService _educationalLevelService;
    public EducationalLevelController(EducationalLevelService educationalLevelService)
    {
        _educationalLevelService = educationalLevelService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var educationalLevels = await _educationalLevelService.GetAllAsync();
            return Ok(educationalLevels);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error" + e.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var educationalLevel = await _educationalLevelService.GetByIdAsync(id);
            return Ok(educationalLevel);
            
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error"+ e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EducationalLevel educationLevel)
    {
        try
        {
            await _educationalLevelService.CreateAsync(educationLevel);
            return Ok("Educational level created successfully");
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: " + e.Message);
        }
    }

    [Authorize]
    [HttpGet("description/{description}")]
    public async Task<IActionResult> GetByDescription(string description)
    {
        try
        {
            var educationalLevel = await _educationalLevelService.GetByDescriptionAsync(description);
            return Ok(educationalLevel);
        }
        catch (Exception e)
        {
            return BadRequest("Unknown error: "+e.Message);
        }
    }
}