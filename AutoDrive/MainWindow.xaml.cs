using System.Windows;
using AutoDrive.Views.Students;
using AutoDrive.Views.Finance;
using AutoDrive.Views.Fleet;
using AutoDrive.Views.Schedule;

namespace AutoDrive
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // По умолчанию показываем модуль "Курсанты"
            ContentArea.Content = new StudentsView();
        }

        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new StudentsView();
        }

        private void BtnFinance_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new PaymentsView();
        }

        private void BtnFleet_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new VehiclesView();
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new ScheduleView();
        }
    }
}