using System;
using System.Threading.Tasks;
using AutoDrive.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AutoDrive.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public PdfGeneratorService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public Task<byte[]> GenerateContractAsync(Student student, decimal totalAmount)
        {
            return Task.Run(() =>
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(40);
                        page.Header().Text("ДОГОВОР НА ОБУЧЕНИЕ В АВТОШКОЛЕ").SemiBold().FontSize(18).AlignCenter();
                        page.Content().PaddingVertical(20).Column(col =>
                        {
                            col.Item().Text($"г. Иркутск, {DateTime.Now:dd.MM.yyyy}").AlignRight();
                            col.Item().PaddingTop(10).Text($"Автошкола ООО «Легаси»... и {student.LastName} {student.FirstName} {student.MiddleName}...");
                            // ... упрощённо, но можно оставить заглушку или полный текст
                        });
                    });
                });
                return document.GeneratePdf();
            });
        }

        public Task<byte[]> GenerateCertificateAsync(Student student)
        {
            return Task.Run(() =>
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(40);
                        page.Content().Column(col =>
                        {
                            col.Item().Text("СВИДЕТЕЛЬСТВО").SemiBold().FontSize(24).AlignCenter();
                            col.Item().PaddingTop(20).Text($"... {student.LastName} {student.FirstName} ...");
                        });
                    });
                });
                return document.GeneratePdf();
            });
        }
    }
}