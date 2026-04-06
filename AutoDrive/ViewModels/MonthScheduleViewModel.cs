using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;
using AutoDrive.Views.Schedule;

namespace AutoDrive.ViewModels
{
    public class MonthScheduleViewModel : ViewModelBase
    {
        private DateTime _currentDate;
        private readonly IMockLessonService _lessonService;

        public ObservableCollection<DayCell> Days { get; } = new();

        public string MonthYear => _currentDate.ToString("MMMM yyyy");

        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }
        public ICommand TodayCommand { get; }
        public ICommand OpenDayCommand { get; }

        public MonthScheduleViewModel(IMockLessonService lessonService)
        {
            _lessonService = lessonService;
            _currentDate = DateTime.Today;
            PreviousMonthCommand = new RelayCommand(_ => ChangeMonth(-1));
            NextMonthCommand = new RelayCommand(_ => ChangeMonth(1));
            TodayCommand = new RelayCommand(_ => GoToToday());
            OpenDayCommand = new RelayCommand(OpenDay);

            GenerateMonth();
        }

        private void ChangeMonth(int delta)
        {
            _currentDate = _currentDate.AddMonths(delta);
            GenerateMonth();
        }

        private void GoToToday()
        {
            _currentDate = DateTime.Today;
            GenerateMonth();
        }

        private async void GenerateMonth()
        {
            Days.Clear();
            var firstOfMonth = new DateTime(_currentDate.Year, _currentDate.Month, 1);
            int startOffset = (firstOfMonth.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)firstOfMonth.DayOfWeek) - 1;
            var startDate = firstOfMonth.AddDays(-startOffset);

            // Загрузим все занятия, чтобы показать индикатор наличия
            var lessons = await _lessonService.GetAllAsync();

            for (int i = 0; i < 42; i++) // 6 недель
            {
                var date = startDate.AddDays(i);
                var hasLessons = lessons.Any(l => l.StartTime.Date == date);
                Days.Add(new DayCell
                {
                    Date = date,
                    DayNumber = date.Day,
                    IsCurrentMonth = date.Month == _currentDate.Month,
                    HasLessons = hasLessons
                });
            }
            OnPropertyChanged(nameof(MonthYear));
        }

        private void OpenDay(object? parameter)
        {
            if (parameter is DateTime date)
            {
                var dialog = new DailyScheduleView(date, _lessonService);
                dialog.Owner = App.Current.MainWindow;
                dialog.ShowDialog();
                GenerateMonth(); // обновим календарь после закрытия диалога
            }
        }
    }

    public class DayCell
    {
        public DateTime Date { get; set; }
        public int DayNumber { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool HasLessons { get; set; }
    }
}