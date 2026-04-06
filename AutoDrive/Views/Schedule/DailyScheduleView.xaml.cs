using AutoDrive.Services;
using AutoDrive.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace AutoDrive.Views.Schedule
{
    public partial class DailyScheduleView : Window
    {
        public DailyScheduleView(DateTime date, IMockLessonService lessonService)
        {
            InitializeComponent();
            var studentService = App.ServiceProvider.GetRequiredService<IMockStudentService>();
            var instructorService = App.ServiceProvider.GetRequiredService<IMockInstructorService>();
            var vehicleService = App.ServiceProvider.GetRequiredService<IMockVehicleService>();
            var viewModel = new DailyScheduleViewModel(date, lessonService, studentService, instructorService, vehicleService);
            DataContext = viewModel;
        }
    }
}