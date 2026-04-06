using AutoDrive.Services;
using AutoDrive.ViewModels;
using AutoDrive.Views.Finance;
using AutoDrive.Views.Fleet;
using AutoDrive.Views.Schedule;
using AutoDrive.Views.Students;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AutoDrive
{
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;

        // Конструктор с DI
        public MainWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            // По умолчанию загружаем модуль "Курсанты"
            ContentArea.Content = _serviceProvider.GetRequiredService<StudentsView>();
        }

        private void BtnStudents_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _serviceProvider.GetRequiredService<StudentsView>(); ;
        }

        private void BtnFinance_Click(object sender, RoutedEventArgs e)
        {
            // Для модуля "Финансы" пока создаём через new, но позже тоже зарегистрируем
            ContentArea.Content = _serviceProvider.GetRequiredService<PaymentsView>();
        }

        private void BtnFleet_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _serviceProvider.GetRequiredService<FleetView>();
        }

        private void BtnSchedule_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _serviceProvider.GetRequiredService<MonthScheduleView>();
        }
    }
}