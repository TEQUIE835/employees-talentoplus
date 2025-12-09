using _1.Application.DTOs.Employees;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using static OfficeOpenXml.ExcelPackage;
using LicenseContext = OfficeOpenXml.LicenseContext;
public class ExcelRead
{

    

    public async Task<List<EmployeeImportDto>> ReadExcelAsync(IFormFile file)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var empleados = new List<EmployeeImportDto>();
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        using var package = new ExcelPackage(stream);
        var worksheet = package.Workbook.Worksheets[0]; // Primera hoja
        int rowCount = worksheet.Dimension.Rows;

        for (int row = 2; row <= rowCount; row++) // asumimos que fila 1 es encabezado
        {
            empleados.Add(new EmployeeImportDto
            {
                Documento = worksheet.Cells[row, 1].Text.Trim(),
                Nombres = worksheet.Cells[row, 2].Text.Trim(),
                Apellidos = worksheet.Cells[row, 3].Text.Trim(),
                FechaNacimiento = DateTime.Parse(worksheet.Cells[row, 4].Text),
                Direccion = worksheet.Cells[row, 5].Text.Trim(),
                Telefono = worksheet.Cells[row, 6].Text.Trim(),
                Email = worksheet.Cells[row, 7].Text.Trim().ToLower(),
                Cargo = worksheet.Cells[row, 8].Text.Trim(),
                Salario = decimal.Parse(worksheet.Cells[row, 9].Text),
                FechaIngreso = DateTime.Parse(worksheet.Cells[row, 10].Text),
                Estado = worksheet.Cells[row, 11].Text.Trim(),
                NivelEducativo = worksheet.Cells[row, 12].Text.Trim(),
                PerfilProfesional = worksheet.Cells[row, 13].Text.Trim(),
                Departamento = worksheet.Cells[row, 14].Text.Trim()
            });
        }

        return empleados;
    }
}