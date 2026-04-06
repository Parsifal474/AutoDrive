using System.Collections.Generic;
using System.Windows;
using AutoDrive.Models;

namespace AutoDrive.Views
{
    public partial class VehicleEditDialog : Window
    {
        public VehicleEditDialog(Vehicle vehicle, List<Instructor> instructors)
        {
            InitializeComponent();
            DataContext = vehicle;
            Instructors = instructors;
            Statuses = new List<string> { "Активен", "Ремонт", "Списано" };
        }

        public List<Instructor> Instructors { get; }
        public List<string> Statuses { get; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var vehicle = DataContext as Vehicle;
            if (vehicle == null || string.IsNullOrWhiteSpace(vehicle.Brand) || string.IsNullOrWhiteSpace(vehicle.Model) || string.IsNullOrWhiteSpace(vehicle.LicensePlate))
            {
                MessageBox.Show("Марка, модель и госномер обязательны!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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