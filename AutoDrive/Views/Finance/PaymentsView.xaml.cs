using System.Windows.Controls;
using AutoDrive.ViewModels;

namespace AutoDrive.Views.Finance
{
    public partial class PaymentsView : UserControl
    {
        public PaymentsView(PaymentsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}