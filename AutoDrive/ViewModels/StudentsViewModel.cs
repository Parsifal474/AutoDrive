using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoDrive.Models;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;
using AutoDrive.Views;   // для StudentEditDialog

namespace AutoDrive.ViewModels
{
    public class StudentsViewModel : ViewModelBase
    {
        private readonly IMockStudentService _studentService;
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

        // Конструктор для DI (без параметров по умолчанию)
        public StudentsViewModel(IMockStudentService studentService)
        {
            _studentService = studentService;
            LoadCommand = new RelayCommand(async _ => await LoadStudents());
            AddCommand = new RelayCommand(async _ => await AddStudent());
            EditCommand = new RelayCommand(async _ => await EditStudent(), _ => SelectedStudent != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteStudent(), _ => SelectedStudent != null);
            _ = LoadStudents();
        }

        private async Task LoadStudents()
        {
            var all = await _studentService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var filtered = all.Where(s => s.LastName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
                                              s.FirstName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) ||
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
    }
}