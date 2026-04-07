using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoDrive.Models;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;


namespace AutoDrive.ViewModels
{
    public class ReportsViewModel : ViewModelBase
    {
        private readonly IMockPaymentService _paymentService;
        private readonly IMockStudentService _studentService;
        private DateTime _startDate;
        private DateTime _endDate;
        private decimal _totalRevenue;
        private ObservableCollection<DebtItem> _debts = new();

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set => SetProperty(ref _totalRevenue, value);
        }

        public ObservableCollection<DebtItem> Debts
        {
            get => _debts;
            set => SetProperty(ref _debts, value);
        }

        public RelayCommand CalculateRevenueCommand { get; }
        public RelayCommand CalculateDebtsCommand { get; }
        public RelayCommand ExportToExcelCommand { get; }

        public ReportsViewModel(IMockPaymentService paymentService, IMockStudentService studentService)
        {
            _paymentService = paymentService;
            _studentService = studentService;

            // Устанавливаем период по умолчанию: текущий месяц
            StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            EndDate = StartDate.AddMonths(1).AddDays(-1);

            CalculateRevenueCommand = new RelayCommand(async _ => await CalculateRevenue());
            CalculateDebtsCommand = new RelayCommand(async _ => await CalculateDebts());
            ExportToExcelCommand = new RelayCommand(async _ => await ExportToExcel());
        }

        private async Task CalculateRevenue()
        {
            var payments = await _paymentService.GetAllAsync();
            var filtered = payments.Where(p => p.PaymentDate >= StartDate && p.PaymentDate <= EndDate);
            TotalRevenue = filtered.Sum(p => p.Amount);
        }

        private async Task CalculateDebts()
        {
            var students = await _studentService.GetAllAsync();
            var payments = await _paymentService.GetAllAsync();
            const decimal fullCourseCost = 25000m; // стоимость обучения, в реальности брать из настроек или из базы

            var debtsList = students.Select(s =>
            {
                var paid = payments.Where(p => p.StudentId == s.Id).Sum(p => p.Amount);
                var debt = fullCourseCost - paid;
                return new DebtItem
                {
                    StudentName = $"{s.LastName} {s.FirstName} {s.MiddleName}",
                    PaidAmount = paid,
                    DebtAmount = debt > 0 ? debt : 0,
                    IsFullyPaid = debt <= 0
                };
            }).Where(d => d.DebtAmount > 0).ToList();

            Debts = new ObservableCollection<DebtItem>(debtsList);
        }

        private async Task ExportToExcel()
        {
            // Установка лицензии EPPlus (для некоммерческого использования)
            ExcelPackage.License.SetNonCommercialOrganization("AutoDrive CRM Project");

            var payments = await _paymentService.GetAllAsync();
            var filtered = payments.Where(p => p.PaymentDate >= StartDate && p.PaymentDate <= EndDate).ToList();
            var students = await _studentService.GetAllAsync();
            var studentDict = students.ToDictionary(s => s.Id);

            using var package = new OfficeOpenXml.ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Платежи");

            // Заголовки
            worksheet.Cells[1, 1].Value = "ID платежа";
            worksheet.Cells[1, 2].Value = "Курсант";
            worksheet.Cells[1, 3].Value = "Сумма";
            worksheet.Cells[1, 4].Value = "Дата";
            worksheet.Cells[1, 5].Value = "Тип";
            worksheet.Cells[1, 6].Value = "Описание";

            int row = 2;
            foreach (var p in filtered)
            {
                worksheet.Cells[row, 1].Value = p.Id;
                var studentName = studentDict.ContainsKey(p.StudentId) ? $"{studentDict[p.StudentId].LastName} {studentDict[p.StudentId].FirstName}" : "—";
                worksheet.Cells[row, 2].Value = studentName;
                worksheet.Cells[row, 3].Value = p.Amount;
                worksheet.Cells[row, 4].Value = p.PaymentDate.ToString("dd.MM.yyyy");
                worksheet.Cells[row, 5].Value = p.PaymentType;
                worksheet.Cells[row, 6].Value = p.Description;
                row++;
            }

            worksheet.Cells.AutoFitColumns();

            var saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                FileName = $"Отчет_платежи_{StartDate:yyyyMMdd}_{EndDate:yyyyMMdd}.xlsx"
            };
            if (saveDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllBytes(saveDialog.FileName, package.GetAsByteArray());
                MessageBox.Show($"Отчёт сохранён: {saveDialog.FileName}", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public class DebtItem
        {
            public string StudentName { get; set; } = string.Empty;
            public decimal PaidAmount { get; set; }
            public decimal DebtAmount { get; set; }
            public bool IsFullyPaid { get; set; }
        }
    }
}