using System.Collections.Generic;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public interface IMockPaymentService
    {
        Task<List<Payment>> GetAllAsync();
        Task<List<Payment>> GetByStudentIdAsync(int studentId);
        Task<Payment?> GetByIdAsync(int id);
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> UpdateAsync(int id, Payment payment);
        Task<bool> DeleteAsync(int id);
    }
}