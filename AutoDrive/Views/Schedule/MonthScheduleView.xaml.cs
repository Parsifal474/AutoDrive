using System.Windows.Controls;
using AutoDrive.ViewModels;

namespace AutoDrive.Views.Schedule
{
    public partial class MonthScheduleView : UserControl
    {
        public MonthScheduleView(MonthScheduleViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}