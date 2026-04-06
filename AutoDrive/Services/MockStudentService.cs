using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public class MockStudentService : IMockStudentService
    {
        private static List<Student> _students = new()
        {
            new Student { Id = 1, LastName = "Иванов", FirstName = "Иван", MiddleName = "Иванович", Phone = "+79001234567", Email = "ivan@example.com", Status = "Теория", ContractNumber = "Д-001" },
            new Student { Id = 2, LastName = "Петрова", FirstName = "Анна", MiddleName = "Сергеевна", Phone = "+79007654321", Email = "anna@example.com", Status = "Вождение", ContractNumber = "Д-002" },
            new Student { Id = 3, LastName = "Сидоров", FirstName = "Пётр", Phone = "+79005555555", Status = "Новый", ContractNumber = "Д-003" }
        };

        public Task<List<Student>> GetAllAsync() => Task.FromResult(_students);

        public Task<Student?> GetByIdAsync(int id) => Task.FromResult(_students.FirstOrDefault(s => s.Id == id));

        public Task<Student> CreateAsync(Student student)
        {
            student.Id = _students.Any() ? _students.Max(s => s.Id) + 1 : 1;
            _students.Add(student);
            return Task.FromResult(student);
        }

        public Task<Student?> UpdateAsync(int id, Student student)
        {
            var existing = _students.FirstOrDefault(s => s.Id == id);
            if (existing == null) return Task.FromResult<Student?>(null);

            existing.LastName = student.LastName;
            existing.FirstName = student.FirstName;
            existing.MiddleName = student.MiddleName;
            existing.Phone = student.Phone;
            existing.Email = student.Email;
            existing.Status = student.Status;
            existing.ContractNumber = student.ContractNumber;
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var student = _students.FirstOrDefault(s => s.Id == id);
            if (student != null)
            {
                _students.Remove(student);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}