using _1.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace talentoPlus.Api.Controllers;
[Authorize]
[ApiController]
[Route("api/importexcel")]
public class ExcelImportController : ControllerBase
{
    private readonly ImportExcelService _importService;
    private readonly ExcelRead _excelService;

    public ExcelImportController(ImportExcelService importService, ExcelRead excelService)
    {
        _importService = importService;
        _excelService = excelService;
    }

    [HttpPost("empleados")]
    public async Task<IActionResult> ImportarExcel(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Archivo no enviado");

        var empleados = await _excelService.ReadExcelAsync(file);
        await _importService.ImportEmployeesAsync(empleados);

        return Ok("Importación completa");
    }
}