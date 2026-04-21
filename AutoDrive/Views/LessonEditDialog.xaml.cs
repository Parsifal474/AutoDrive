using System;
using System.Collections.Generic;
using System.Windows;
using AutoDrive.Models;

namespace AutoDrive.Views
{
    public partial class LessonEditDialog : Window
    {
        private Lesson _lesson;
        private int _startHour, _startMinute, _endHour, _endMinute;

        public LessonEditDialog(Lesson lesson, List<Student> students, List<Instructor> instructors, List<Vehicle> vehicles)
        {
            InitializeComponent();
            _lesson = lesson;
            DataContext = this; // Чтобы привязки шли к свойствам этого окна
            Students = students;
            Instructors = instructors;
            Vehicles = vehicles;

            // Инициализация свойств времени
            StartHour = lesson.StartTime.Hour;
            StartMinute = lesson.StartTime.Minute;
            EndHour = lesson.EndTime.Hour;
            EndMinute = lesson.EndTime.Minute;
        }

        public List<Student> Students { get; }
        public List<Instructor> Instructors { get; }
        public List<Vehicle> Vehicles { get; }

        public int StartHour
        {
            get => _startHour;
            set
            {
                _startHour = value;
                UpdateStartTime();
            }
        }

        public int StartMinute
        {
            get => _startMinute;
            set
            {
                _startMinute = value;
                UpdateStartTime();
            }
        }

        public int EndHour
        {
            get => _endHour;
            set
            {
                _endHour = value;
                UpdateEndTime();
            }
        }

        public int EndMinute
        {
            get => _endMinute;
            set
            {
                _endMinute = value;
                UpdateEndTime();
            }
        }

        private void UpdateStartTime()
        {
            var newStart = new DateTime(_lesson.StartTime.Year, _lesson.StartTime.Month, _lesson.StartTime.Day, StartHour, StartMinute, 0);
            _lesson.StartTime = newStart;
        }

        private void UpdateEndTime()
        {
            var newEnd = new DateTime(_lesson.EndTime.Year, _lesson.EndTime.Month, _lesson.EndTime.Day, EndHour, EndMinute, 0);
            _lesson.EndTime = newEnd;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_lesson.StudentId == 0 || _lesson.InstructorId == 0 || _lesson.VehicleId == 0)
            {
                MessageBox.Show("Заполните все обязательные поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}