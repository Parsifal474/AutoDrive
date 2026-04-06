using System;

namespace AutoDrive.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public virtual Student? Student { get; set; } // навигационное свойство
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public string? Description { get; set; }
        public string PaymentType { get; set; } = "Наличные"; // Наличные, Карта, Перевод
        public bool IsInstallment { get; set; } // часть рассрочки?
    }
}