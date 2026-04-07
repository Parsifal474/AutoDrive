using System;
using System.IO;
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
            // Лицензия для некоммерческого использования
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
                        page.DefaultTextStyle(x => x.FontSize(12));

                        page.Header()
                            .Text("ДОГОВОР НА ОБУЧЕНИЕ В АВТОШКОЛЕ")
                            .SemiBold().FontSize(18).AlignCenter();

                        page.Content()
                            .PaddingVertical(20)
                            .Column(col =>
                            {
                                col.Item().Text($"г. Иркутск, {DateTime.Now:dd.MM.yyyy}").AlignRight();
                                col.Item().PaddingTop(10).Text($"Автошкола ООО «Легаси», именуемая в дальнейшем «Исполнитель», в лице директора Санковец Д.А., с одной стороны, и {student.LastName} {student.FirstName} {student.MiddleName}, именуемый в дальнейшем «Курсант», с другой стороны, заключили настоящий договор о нижеследующем:");
                                col.Item().PaddingTop(10).Text("1. ПРЕДМЕТ ДОГОВОРА").Bold();
                                col.Item().Text("Исполнитель обязуется оказать Курсанту образовательные услуги по подготовке водителей категории «B», а Курсант обязуется оплатить обучение в размере и порядке, установленном настоящим договором.");
                                col.Item().PaddingTop(10).Text("2. СТОИМОСТЬ И ПОРЯДОК ОПЛАТЫ").Bold();
                                col.Item().Text($"Общая стоимость обучения составляет {totalAmount:N2} рублей. Оплата производится в рассрочку согласно графику платежей (приложение №1).");
                                col.Item().PaddingTop(10).Text("3. ПРАВА И ОБЯЗАННОСТИ СТОРОН").Bold();
                                col.Item().Text("Исполнитель обязуется предоставить учебные материалы, транспорт, инструкторов. Курсант обязуется посещать занятия и соблюдать правила внутреннего распорядка.");
                                col.Item().PaddingTop(10).Text("4. ОТВЕТСТВЕННОСТЬ СТОРОН").Bold();
                                col.Item().Text("За неисполнение обязательств стороны несут ответственность согласно законодательству РФ.");
                                col.Item().PaddingTop(10).Text("5. ПРОЧИЕ УСЛОВИЯ").Bold();
                                col.Item().Text("Договор вступает в силу с даты подписания и действует до полного исполнения обязательств.");
                            });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span("Страница ");
                                x.CurrentPageNumber();
                            });
                    });
                });
                var pdfBytes = document.GeneratePdf();
                return pdfBytes;
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
                        page.DefaultTextStyle(x => x.FontSize(14));

                        page.Content().Column(col =>
                        {
                            col.Item().Text("СВИДЕТЕЛЬСТВО").SemiBold().FontSize(24).AlignCenter();
                            col.Item().PaddingTop(20).Text($"Настоящим подтверждается, что {student.LastName} {student.FirstName} {student.MiddleName}").AlignCenter();
                            col.Item().PaddingTop(10).Text($"успешно завершил(а) полный курс обучения в автошколе ООО «Легаси» по программе подготовки водителей категории «B».").AlignCenter();
                            col.Item().PaddingTop(10).Text($"Свидетельство действительно при предъявлении документа, удостоверяющего личность.").AlignCenter();
                            col.Item().PaddingTop(30).Text($"Дата выдачи: {DateTime.Now:dd.MM.yyyy}").AlignRight();
                            col.Item().PaddingTop(10).Text("Директор ____________________ /Санковец Д.А./").AlignRight();
                        });
                    });
                });
                return document.GeneratePdf();
            });
        }
    }
}