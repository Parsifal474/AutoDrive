using AutoDrive.Models;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;
using AutoDrive.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoDrive.ViewModels
{
    public class StudentsViewModel : ViewModelBase
    {
        private readonly IMockStudentService _studentService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private ObservableCollection<Student> _students = new();
        private Student? _selectedStudent;
        private string _searchText = string.Empty;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        public Student? SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _ = LoadStudents();
            }
        }

        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand ContractCommand { get; }      // договор
        public RelayCommand CertificateCommand { get; }  // свидетельство

        public StudentsViewModel(IMockStudentService studentService, IPdfGeneratorService pdfGeneratorService)
        {
            _studentService = studentService;
            _pdfGeneratorService = pdfGeneratorService;

            LoadCommand = new RelayCommand(async _ => await LoadStudents());
            AddCommand = new RelayCommand(async _ => await AddStudent());
            EditCommand = new RelayCommand(async _ => await EditStudent(), _ => SelectedStudent != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteStudent(), _ => SelectedStudent != null);
            ContractCommand = new RelayCommand(async _ => await GenerateContract(), _ => SelectedStudent != null);
            CertificateCommand = new RelayCommand(async _ => await GenerateCertificate(), _ => SelectedStudent != null);

            _ = LoadStudents();
        }

        private async Task LoadStudents()
        {
            var all = await _studentService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var filtered = all.Where(s => s.LastName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                              s.FirstName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                              s.Phone.Contains(SearchText)).ToList();
                Students = new ObservableCollection<Student>(filtered);
            }
            else
            {
                Students = new ObservableCollection<Student>(all);
            }
        }

        private async Task AddStudent()
        {
            var newStudent = new Student();
            var dialog = new StudentEditDialog(newStudent);
            if (dialog.ShowDialog() == true)
            {
                await _studentService.CreateAsync(newStudent);
                await LoadStudents();
            }
        }

        private async Task EditStudent()
        {
            if (SelectedStudent == null) return;
            var copy = new Student
            {
                Id = SelectedStudent.Id,
                LastName = SelectedStudent.LastName,
                FirstName = SelectedStudent.FirstName,
                MiddleName = SelectedStudent.MiddleName,
                Phone = SelectedStudent.Phone,
                Email = SelectedStudent.Email,
                Status = SelectedStudent.Status,
                ContractNumber = SelectedStudent.ContractNumber
            };
            var dialog = new StudentEditDialog(copy);
            if (dialog.ShowDialog() == true)
            {
                await _studentService.UpdateAsync(copy.Id, copy);
                await LoadStudents();
            }
        }

        private async Task DeleteStudent()
        {
            if (SelectedStudent == null) return;
            if (MessageBox.Show($"Удалить курсанта {SelectedStudent.LastName} {SelectedStudent.FirstName}?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _studentService.DeleteAsync(SelectedStudent.Id);
                await LoadStudents();
            }
        }

        // Генерация договора
        private async Task GenerateContract()
        {
            if (SelectedStudent == null) return;
            // Здесь должна быть реальная сумма из базы данных или вычисленная
            decimal totalAmount = 25000m; // пример: стоимость обучения
            var pdfBytes = await _pdfGeneratorService.GenerateContractAsync(SelectedStudent, totalAmount);
            SaveAndOpenPdf(pdfBytes, $"Договор_{SelectedStudent.LastName}_{SelectedStudent.FirstName}.pdf");
        }

        // Генерация свидетельства
        private async Task GenerateCertificate()
        {
            if (SelectedStudent == null) return;
            var pdfBytes = await _pdfGeneratorService.GenerateCertificateAsync(SelectedStudent);
            SaveAndOpenPdf(pdfBytes, $"Свидетельство_{SelectedStudent.LastName}_{SelectedStudent.FirstName}.pdf");
        }

        private void SaveAndOpenPdf(byte[] pdfBytes, string fileName)
        {
            var tempFile = Path.Combine(Path.GetTempPath(), fileName);
            File.WriteAllBytes(tempFile, pdfBytes);
            // Открываем PDF в программе по умолчанию
            Process.Start(new ProcessStartInfo(tempFile) { UseShellExecute = true });
        }
    }
}