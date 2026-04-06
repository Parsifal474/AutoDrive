using System.Windows.Controls;
using AutoDrive.ViewModels;

namespace AutoDrive.Views.Students
{
    public partial class StudentsView : UserControl
    {
        public StudentsView(StudentsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}