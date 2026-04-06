using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public class MockInstructorService : IMockInstructorService
    {
        private static List<Instructor> _instructors = new()
        {
            new Instructor { Id = 1, LastName = "Смирнов", FirstName = "Иван", MiddleName = "Петрович", Phone = "+79111234567", Email = "ivan@example.com", HourlyRate = 500, IsActive = true },
            new Instructor { Id = 2, LastName = "Кузнецова", FirstName = "Ольга", MiddleName = "Алексеевна", Phone = "+79117654321", Email = "olga@example.com", HourlyRate = 550, IsActive = true }
        };

        public Task<List<Instructor>> GetAllAsync() => Task.FromResult(_instructors);

        public Task<Instructor?> GetByIdAsync(int id) => Task.FromResult(_instructors.FirstOrDefault(i => i.Id == id));

        public Task<Instructor> CreateAsync(Instructor instructor)
        {
            instructor.Id = _instructors.Any() ? _instructors.Max(i => i.Id) + 1 : 1;
            _instructors.Add(instructor);
            return Task.FromResult(instructor);
        }

        public Task<Instructor?> UpdateAsync(int id, Instructor instructor)
        {
            var existing = _instructors.FirstOrDefault(i => i.Id == id);
            if (existing == null) return Task.FromResult<Instructor?>(null);
            existing.LastName = instructor.LastName;
            existing.FirstName = instructor.FirstName;
            existing.MiddleName = instructor.MiddleName;
            existing.Phone = instructor.Phone;
            existing.Email = instructor.Email;
            existing.HourlyRate = instructor.HourlyRate;
            existing.IsActive = instructor.IsActive;
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var instructor = _instructors.FirstOrDefault(i => i.Id == id);
            if (instructor != null)
            {
                _instructors.Remove(instructor);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}