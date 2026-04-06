using System.Collections.Generic;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public interface IMockStudentService
    {
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task<Student> CreateAsync(Student student);
        Task<Student?> UpdateAsync(int id, Student student);
        Task<bool> DeleteAsync(int id);
    }
}