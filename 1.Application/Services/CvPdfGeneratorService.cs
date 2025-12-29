using System.Reflection.Metadata;
using _2.Domain.Entities;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;


namespace _1.Application.Services;

public class CvPdfGeneratorService
{
    public byte[] GenerateCvPdfBytes(Employee employee)
    {
        using var stream = new MemoryStream();

        Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Size(PageSizes.A4);

                    page.Header()
                        .Text($"{employee.FirstName} {employee.LastName}")
                        .FontSize(24)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Text($"Correo: {employee.Email}");
                            column.Item().Text($"Teléfono: {employee.PhoneNumber}");
                            column.Item().Text($"Dirección: {employee.Address}");
                            column.Item().Text($"Nivel educativo: {employee.EducationalLevel.Description}");
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(txt =>
                        {
                            txt.Span("Página ");     
                            txt.CurrentPageNumber();   
                            txt.Span(" / ");           
                            txt.TotalPages();          
                        });
                });
            })
            .GeneratePdf(stream);

        return stream.ToArray();
    }

}