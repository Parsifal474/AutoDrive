using AutoDrive.Data;
using AutoDrive.Services;
using AutoDrive.ViewModels;
using AutoDrive.Views;
using AutoDrive.Views.Finance;
using AutoDrive.Views.Fleet;
using AutoDrive.Views.Schedule;
using AutoDrive.Views.Students;
using Microsoft.EntityFrameworkCore;
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

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // --- База данных PostgreSQL ---
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=AutoDriveCRM;Username=postgres;Password=ваш_пароль"));

            // --- Реальные сервисы (заменяют моки) ---
            services.AddScoped<IMockStudentService, DbStudentService>();
            services.AddScoped<IMockPaymentService, DbPaymentService>();
            services.AddScoped<IMockInstructorService, DbInstructorService>();
            services.AddScoped<IMockVehicleService, DbVehicleService>();
            services.AddScoped<IMockLessonService, DbLessonService>();

            // --- Прочие сервисы ---
            services.AddSingleton<IPdfGeneratorService, PdfGeneratorService>();

            // --- ViewModels ---
            services.AddTransient<StudentsViewModel>();
            services.AddTransient<PaymentsViewModel>();
            services.AddTransient<FleetViewModel>();
            services.AddTransient<MonthScheduleViewModel>();
            services.AddTransient<ReportsViewModel>();

            // --- View и окна ---
            services.AddTransient<MainWindow>();
            services.AddTransient<StudentsView>();
            services.AddTransient<PaymentsView>();
            services.AddTransient<FleetView>();
            services.AddTransient<MonthScheduleView>();
            services.AddTransient<StudentEditDialog>();
            services.AddTransient<PaymentEditDialog>();
            services.AddTransient<VehicleEditDialog>();
            services.AddTransient<InstructorEditDialog>();
            services.AddTransient<LessonEditDialog>();
        }
    }
}