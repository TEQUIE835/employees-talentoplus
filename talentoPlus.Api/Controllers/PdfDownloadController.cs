using _1.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace talentoPlus.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PdfDownloadController : ControllerBase
{
    private readonly EmployeeServices _employeeServices;
    private readonly CvPdfGeneratorService _pdfService;

    public PdfDownloadController(EmployeeServices employeeServices, CvPdfGeneratorService pdfService)
    {
        _employeeServices = employeeServices;
        _pdfService = pdfService;
    }
    
    [HttpGet("download-cv/{employeeId}")]
    public async Task<IActionResult> DownloadCv(int employeeId)
    {
        var employee = await _employeeServices.GetByIdAsync(employeeId);
        if (employee == null) return NotFound();

        var pdfBytes = _pdfService.GenerateCvPdfBytes(employee);

        return File(pdfBytes, "application/pdf", $"{employee.FirstName}_{employee.LastName}_CV.pdf");
    }

}