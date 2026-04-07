using AutoDrive.Services;
using AutoDrive.ViewModels;
using AutoDrive.Views;
using AutoDrive.Views.Finance;
using AutoDrive.Views.Fleet;
using AutoDrive.Views.Schedule;
using AutoDrive.Views.Students;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace AutoDrive
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            // Получаем главное окно из контейнера и показываем
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем сервисы
            services.AddSingleton<IMockStudentService, MockStudentService>();

            // Регистрируем ViewModels
            services.AddTransient<StudentsViewModel>();

            // Регистрируем окна и UserControl (если они имеют зависимости в конструкторе)
            services.AddTransient<MainWindow>();
            services.AddTransient<StudentsView>();
            services.AddTransient<StudentEditDialog>();
            // Остальные View (PaymentsView, VehiclesView и т.д.) пока без зависимостей, можно не регистрировать
            // или зарегистрировать как Transient, если позже понадобятся

            // модуль финансы
            services.AddSingleton<IMockPaymentService, MockPaymentService>();
            services.AddTransient<PaymentsViewModel>();
            services.AddTransient<PaymentsView>();
            services.AddTransient<PaymentEditDialog>(); // если нужно

            // модуль инструктор и автомобили
            services.AddSingleton<IMockVehicleService, MockVehicleService>();
            services.AddSingleton<IMockInstructorService, MockInstructorService>();
            services.AddTransient<FleetViewModel>();
            services.AddTransient<FleetView>();
            services.AddTransient<VehicleEditDialog>();
            services.AddTransient<InstructorEditDialog>();

            //каледарь
            services.AddSingleton<IMockLessonService, MockLessonService>();
            services.AddTransient<MonthScheduleViewModel>();
            services.AddTransient<MonthScheduleView>();

            //pdf генератор
            services.AddSingleton<IPdfGeneratorService, PdfGeneratorService>();

            services.AddSingleton<IMockStudentService, MockStudentService>();
            services.AddSingleton<IMockPaymentService, MockPaymentService>();
            services.AddTransient<PaymentsViewModel>();
            services.AddTransient<PaymentsView>();
        }
    }
}