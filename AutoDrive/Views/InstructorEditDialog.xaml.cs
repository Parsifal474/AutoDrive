using System.Windows;
using AutoDrive.Models;

namespace AutoDrive.Views
{
    public partial class InstructorEditDialog : Window
    {
        public InstructorEditDialog(Instructor instructor)
        {
            InitializeComponent();
            DataContext = instructor;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var instructor = DataContext as Instructor;
            if (instructor == null || string.IsNullOrWhiteSpace(instructor.LastName) || string.IsNullOrWhiteSpace(instructor.FirstName) || string.IsNullOrWhiteSpace(instructor.Phone))
            {
                MessageBox.Show("Фамилия, имя и телефон обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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