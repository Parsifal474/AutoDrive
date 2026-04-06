using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public class MockPaymentService : IMockPaymentService
    {
        private static List<Payment> _payments = new()
        {
            new Payment { Id = 1, StudentId = 1, Amount = 15000, PaymentDate = new DateTime(2026, 1, 20), PaymentType = "Наличные", Description = "Оплата за обучение" },
            new Payment { Id = 2, StudentId = 1, Amount = 5000, PaymentDate = new DateTime(2026, 2, 15), PaymentType = "Карта", Description = "Доплата", IsInstallment = true },
            new Payment { Id = 3, StudentId = 2, Amount = 20000, PaymentDate = new DateTime(2026, 1, 25), PaymentType = "Наличные", Description = "Полная оплата" }
        };

        public Task<List<Payment>> GetAllAsync() => Task.FromResult(_payments);

        public Task<List<Payment>> GetByStudentIdAsync(int studentId) =>
            Task.FromResult(_payments.Where(p => p.StudentId == studentId).ToList());

        public Task<Payment?> GetByIdAsync(int id) =>
            Task.FromResult(_payments.FirstOrDefault(p => p.Id == id));

        public Task<Payment> CreateAsync(Payment payment)
        {
            payment.Id = _payments.Any() ? _payments.Max(p => p.Id) + 1 : 1;
            _payments.Add(payment);
            return Task.FromResult(payment);
        }

        public Task<Payment?> UpdateAsync(int id, Payment payment)
        {
            var existing = _payments.FirstOrDefault(p => p.Id == id);
            if (existing == null) return Task.FromResult<Payment?>(null);
            existing.StudentId = payment.StudentId;
            existing.Amount = payment.Amount;
            existing.PaymentDate = payment.PaymentDate;
            existing.Description = payment.Description;
            existing.PaymentType = payment.PaymentType;
            existing.IsInstallment = payment.IsInstallment;
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var payment = _payments.FirstOrDefault(p => p.Id == id);
            if (payment != null)
            {
                _payments.Remove(payment);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}