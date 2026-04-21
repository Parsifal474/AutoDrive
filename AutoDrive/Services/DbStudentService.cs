using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Services
{
    public class DbStudentService : IMockStudentService
    {
        private readonly AppDbContext _context;

        public DbStudentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Student> CreateAsync(Student student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<Student?> UpdateAsync(int id, Student student)
        {
            var existing = await _context.Students.FindAsync(id);
            if (existing == null) return null;

            existing.LastName = student.LastName;
            existing.FirstName = student.FirstName;
            existing.MiddleName = student.MiddleName;
            existing.Phone = student.Phone;
            existing.Email = student.Email;
            existing.Status = student.Status;
            existing.ContractNumber = student.ContractNumber;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}