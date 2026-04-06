using System;

namespace AutoDrive.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;      // Марка
        public string Model { get; set; } = string.Empty;      // Модель
        public string LicensePlate { get; set; } = string.Empty; // Госномер
        public int Year { get; set; }                          // Год выпуска
        public DateTime? InsuranceExpiry { get; set; }         // Срок ОСАГО/КАСКО
        public DateTime? InspectionExpiry { get; set; }        // Срок диагностической карты
        public DateTime? MaintenanceDue { get; set; }          // Дата следующего ТО
        public string Status { get; set; } = "Активен";        // Активен, В ремонте, Списано
        public int? InstructorId { get; set; }                 // Закреплённый инструктор
        public virtual Instructor? Instructor { get; set; }
    }
}