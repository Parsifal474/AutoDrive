using System.Windows.Controls;
using AutoDrive.ViewModels;

namespace AutoDrive.Views.Fleet
{
    public partial class FleetView : UserControl
    {
        public FleetView(FleetViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}