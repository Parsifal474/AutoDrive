using System.Collections.Generic;
using System.Windows;
using AutoDrive.Models;

namespace AutoDrive.Views
{
    public partial class LessonEditDialog : Window
    {
        public LessonEditDialog(Lesson lesson, List<Student> students, List<Instructor> instructors, List<Vehicle> vehicles)
        {
            InitializeComponent();
            DataContext = lesson;
            Students = students;
            Instructors = instructors;
            Vehicles = vehicles;
        }

        public List<Student> Students { get; }
        public List<Instructor> Instructors { get; }
        public List<Vehicle> Vehicles { get; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var lesson = DataContext as Lesson;
            if (lesson == null || lesson.StudentId == 0 || lesson.InstructorId == 0 || lesson.VehicleId == 0)
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