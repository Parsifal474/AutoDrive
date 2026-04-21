using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Services
{
    public class DbInstructorService : IMockInstructorService
    {
        private readonly AppDbContext _context;

        public DbInstructorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Instructor>> GetAllAsync()
        {
            return await _context.Instructors.ToListAsync();
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            return await _context.Instructors.FindAsync(id);
        }

        public async Task<Instructor> CreateAsync(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
            return instructor;
        }

        public async Task<Instructor?> UpdateAsync(int id, Instructor instructor)
        {
            var existing = await _context.Instructors.FindAsync(id);
            if (existing == null) return null;

            existing.LastName = instructor.LastName;
            existing.FirstName = instructor.FirstName;
            existing.MiddleName = instructor.MiddleName;
            existing.Phone = instructor.Phone;
            existing.Email = instructor.Email;
            existing.HourlyRate = instructor.HourlyRate;
            existing.IsActive = instructor.IsActive;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) return false;

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}