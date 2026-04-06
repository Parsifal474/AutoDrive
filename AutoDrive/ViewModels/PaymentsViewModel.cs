using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoDrive.Models;
using AutoDrive.Services;
using AutoDrive.ViewModels.Base;
using AutoDrive.Views;

namespace AutoDrive.ViewModels
{
    public class PaymentsViewModel : ViewModelBase
    {
        private readonly IMockPaymentService _paymentService;
        private readonly IMockStudentService _studentService;
        private ObservableCollection<Payment> _payments = new();
        private Payment? _selectedPayment;
        private string _searchText = string.Empty;

        public ObservableCollection<Payment> Payments
        {
            get => _payments;
            set => SetProperty(ref _payments, value);
        }

        public Payment? SelectedPayment
        {
            get => _selectedPayment;
            set => SetProperty(ref _selectedPayment, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                _ = LoadPayments();
            }
        }

        public RelayCommand LoadCommand { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand EditCommand { get; }
        public RelayCommand DeleteCommand { get; }

        public PaymentsViewModel(IMockPaymentService paymentService, IMockStudentService studentService)
        {
            _paymentService = paymentService;
            _studentService = studentService;
            LoadCommand = new RelayCommand(async _ => await LoadPayments());
            AddCommand = new RelayCommand(async _ => await AddPayment());
            EditCommand = new RelayCommand(async _ => await EditPayment(), _ => SelectedPayment != null);
            DeleteCommand = new RelayCommand(async _ => await DeletePayment(), _ => SelectedPayment != null);
            _ = LoadPayments();
        }

        private async Task LoadPayments()
        {
            var all = await _paymentService.GetAllAsync();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                // Поиск по описанию или по фамилии курсанта (через Student)
                var students = await _studentService.GetAllAsync();
                var filtered = all.Where(p =>
                    (p.Description?.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) == true) ||
                    (students.FirstOrDefault(s => s.Id == p.StudentId)?.LastName.Contains(SearchText, System.StringComparison.OrdinalIgnoreCase) == true)
                ).ToList();
                Payments = new ObservableCollection<Payment>(filtered);
            }
            else
            {
                Payments = new ObservableCollection<Payment>(all);
            }
        }

        private async Task AddPayment()
        {
            var newPayment = new Payment { PaymentDate = System.DateTime.Now };
            var dialog = new PaymentEditDialog(newPayment, await _studentService.GetAllAsync());
            if (dialog.ShowDialog() == true)
            {
                await _paymentService.CreateAsync(newPayment);
                await LoadPayments();
            }
        }

        private async Task EditPayment()
        {
            if (SelectedPayment == null) return;
            var copy = new Payment
            {
                Id = SelectedPayment.Id,
                StudentId = SelectedPayment.StudentId,
                Amount = SelectedPayment.Amount,
                PaymentDate = SelectedPayment.PaymentDate,
                Description = SelectedPayment.Description,
                PaymentType = SelectedPayment.PaymentType,
                IsInstallment = SelectedPayment.IsInstallment
            };
            var dialog = new PaymentEditDialog(copy, await _studentService.GetAllAsync());
            if (dialog.ShowDialog() == true)
            {
                await _paymentService.UpdateAsync(copy.Id, copy);
                await LoadPayments();
            }
        }

        private async Task DeletePayment()
        {
            if (SelectedPayment == null) return;
            if (MessageBox.Show($"Удалить платёж на сумму {SelectedPayment.Amount} руб.?",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                await _paymentService.DeleteAsync(SelectedPayment.Id);
                await LoadPayments();
            }
        }
    }
}