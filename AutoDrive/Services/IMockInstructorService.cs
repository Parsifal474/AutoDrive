using System.Collections.Generic;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public interface IMockInstructorService
    {
        Task<List<Instructor>> GetAllAsync();
        Task<Instructor?> GetByIdAsync(int id);
        Task<Instructor> CreateAsync(Instructor instructor);
        Task<Instructor?> UpdateAsync(int id, Instructor instructor);
        Task<bool> DeleteAsync(int id);
    }
}