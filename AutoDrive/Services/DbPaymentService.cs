using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Services
{
    public class DbPaymentService : IMockPaymentService
    {
        private readonly AppDbContext _context;

        public DbPaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetAllAsync()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<List<Payment>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Payments
                .Where(p => p.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }

        public async Task<Payment> CreateAsync(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment?> UpdateAsync(int id, Payment payment)
        {
            var existing = await _context.Payments.FindAsync(id);
            if (existing == null) return null;

            existing.StudentId = payment.StudentId;
            existing.Amount = payment.Amount;
            existing.PaymentDate = payment.PaymentDate;
            existing.Description = payment.Description;
            existing.PaymentType = payment.PaymentType;
            existing.IsInstallment = payment.IsInstallment;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}