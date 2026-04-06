using AutoDrive.Models;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;
using AutoDrive.Views;
using AutoDrive.Views.Schedule;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoDrive.ViewModels
{
    public class DailyScheduleViewModel : ViewModelBase
    {
        private readonly IMockLessonService _lessonService;
        private readonly IMockStudentService _studentService;
        private readonly IMockInstructorService _instructorService;
        private readonly IMockVehicleService _vehicleService;
        private DateTime _selectedDate;
        private ObservableCollection<LessonDisplay> _lessons = new();
        private LessonDisplay? _selectedLesson;

        public ObservableCollection<LessonDisplay> Lessons
        {
            get => _lessons;
            set => SetProperty(ref _lessons, value);
        }

        public LessonDisplay? SelectedLesson
        {
            get => _selectedLesson;
            set => SetProperty(ref _selectedLesson, value);
        }

        public string DateTitle => _selectedDate.ToString("dddd, dd MMMM yyyy");

        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }
        public RelayCommand CloseCommand { get; }

        public DailyScheduleViewModel(
            DateTime date,
            IMockLessonService lessonService,
            IMockStudentService studentService,
            IMockInstructorService instructorService,
            IMockVehicleService vehicleService)
        {
            _selectedDate = date;
            _lessonService = lessonService;
            _studentService = studentService;
            _instructorService = instructorService;
            _vehicleService = vehicleService;

            AddCommand = new RelayCommand(async _ => await AddLesson());
            EditCommand = new RelayCommand(async _ => await EditLesson(), _ => SelectedLesson != null);
            DeleteCommand = new RelayCommand(async _ => await DeleteLesson(), _ => SelectedLesson != null);
            CloseCommand = new RelayCommand(_ => Close());

            _ = LoadLessons();
        }

        private async Task LoadLessons()
        {
            var lessons = await _lessonService.GetAllAsync();
            var students = await _studentService.GetAllAsync();
            var instructors = await _instructorService.GetAllAsync();
            var vehicles = await _vehicleService.GetAllAsync();

            var studentDict = students.ToDictionary(s => s.Id);
            var instructorDict = instructors.ToDictionary(i => i.Id);
            var vehicleDict = vehicles.ToDictionary(v => v.Id);

            var dayLessons = lessons
                .Where(l => l.StartTime.Date == _selectedDate.Date)
                .Select(l => new LessonDisplay
                {
                    Lesson = l,
                    StudentName = studentDict.ContainsKey(l.StudentId) ? $"{studentDict[l.StudentId].LastName} {studentDict[l.StudentId].FirstName}" : "—",
                    InstructorName = instructorDict.ContainsKey(l.InstructorId) ? $"{instructorDict[l.InstructorId].LastName} {instructorDict[l.InstructorId].FirstName}" : "—",
                    VehicleTitle = vehicleDict.ContainsKey(l.VehicleId) ? $"{vehicleDict[l.VehicleId].Brand} {vehicleDict[l.VehicleId].Model} ({vehicleDict[l.VehicleId].LicensePlate})" : "—"
                })
                .OrderBy(d => d.Lesson.StartTime)
                .ToList();

            Lessons = new ObservableCollection<LessonDisplay>(dayLessons);
        }

        private async Task AddLesson()
        {
            var newLesson = new Lesson
            {
                StartTime = _selectedDate.Date.AddHours(10),
                EndTime = _selectedDate.Date.AddHours(11),
                StudentId = 1,
                InstructorId = 1,
                VehicleId = 1
            };
            var students = await _studentService.GetAllAsync();
            var instructors = await _instructorService.GetAllAsync();
            var vehicles = await _vehicleService.GetAllAsync();

            var dialog = new LessonEditDialog(newLesson, students, instructors, vehicles);
            if (dialog.ShowDialog() == true)
            {
                if (await _lessonService.IsAvailableAsync(newLesson))
                {
                    await _lessonService.CreateAsync(newLesson);
                    await LoadLessons();
                }
                else
                {
                    MessageBox.Show("Конфликт: инструктор или автомобиль уже заняты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private async Task EditLesson()
        {
            if (SelectedLesson == null) return;
            var copy = new Lesson
            {
                Id = SelectedLesson.Lesson.Id,
                StudentId = SelectedLesson.Lesson.StudentId,
                InstructorId = SelectedLesson.Lesson.InstructorId,
                VehicleId = SelectedLesson.Lesson.VehicleId,
                StartTime = SelectedLesson.Lesson.StartTime,
                EndTime = SelectedLesson.Lesson.EndTime,
                Note = SelectedLesson.Lesson.Note
            };
            var students = await _studentService.GetAllAsync();
            var instructors = await _instructorService.GetAllAsync();
            var vehicles = await _vehicleService.GetAllAsync();

            var dialog = new LessonEditDialog(copy, students, instructors, vehicles);
            if (dialog.ShowDialog() == true)
            {
                if (await _lessonService.IsAvailableAsync(copy))
                {
                    await _lessonService.UpdateAsync(copy.Id, copy);
                    await LoadLessons();
                }
                else
                {
                    MessageBox.Show("Конфликт: инструктор или автомобиль уже заняты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private async Task DeleteLesson()
        {
            if (SelectedLesson == null) return;
            if (MessageBox.Show("Удалить выбранное занятие?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _lessonService.DeleteAsync(SelectedLesson.Lesson.Id);
                await LoadLessons();
            }
        }

        private void Close()
        {
            if (System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this) is System.Windows.Window win)
                win.DialogResult = true;
        }
    }

    public class LessonDisplay
    {
        public Lesson Lesson { get; set; } = null!;
        public string StudentName { get; set; } = string.Empty;
        public string InstructorName { get; set; } = string.Empty;
        public string VehicleTitle { get; set; } = string.Empty;
    }
}