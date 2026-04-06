using System.Windows;
using AutoDrive.Models;

namespace AutoDrive.Views
{
    public partial class StudentEditDialog : Window
    {
        public StudentEditDialog(Student student)
        {
            InitializeComponent();
            DataContext = student;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var student = DataContext as Student;
            if (string.IsNullOrWhiteSpace(student?.LastName) || string.IsNullOrWhiteSpace(student.FirstName) || string.IsNullOrWhiteSpace(student.Phone))
            {
                MessageBox.Show("Фамилия, Имя и Телефон обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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