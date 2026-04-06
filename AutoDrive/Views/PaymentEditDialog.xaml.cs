using System.Collections.Generic;
using System.Windows;
using AutoDrive.Models;

namespace AutoDrive.Views
{
    public partial class PaymentEditDialog : Window
    {
        public PaymentEditDialog(Payment payment, List<Student> students)
        {
            InitializeComponent();
            DataContext = payment;
            Students = students;
            PaymentTypes = new List<string> { "Наличные", "Карта", "Перевод" };
            // Привязка вспомогательных списков
            this.Loaded += (s, e) =>
            {
                var combo = this.FindName("ComboStudent") as System.Windows.Controls.ComboBox;
                if (combo != null) combo.ItemsSource = Students;
                var typeCombo = this.FindName("ComboPaymentType") as System.Windows.Controls.ComboBox;
                if (typeCombo != null) typeCombo.ItemsSource = PaymentTypes;
            };
        }

        public List<Student> Students { get; }
        public List<string> PaymentTypes { get; }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var payment = DataContext as Payment;
            if (payment == null || payment.StudentId == 0 || payment.Amount <= 0)
            {
                MessageBox.Show("Выберите курсанта и укажите сумму больше 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}