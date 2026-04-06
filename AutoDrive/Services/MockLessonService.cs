using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public class MockLessonService : IMockLessonService
    {
        private static List<Lesson> _lessons = new()
        {
            new Lesson { Id = 1, StudentId = 1, InstructorId = 1, VehicleId = 1, StartTime = new DateTime(2026, 4, 6, 10, 0, 0), EndTime = new DateTime(2026, 4, 6, 11, 0, 0), Note = "Первое занятие" },
            new Lesson { Id = 2, StudentId = 2, InstructorId = 2, VehicleId = 2, StartTime = new DateTime(2026, 4, 7, 14, 0, 0), EndTime = new DateTime(2026, 4, 7, 15, 30, 0), Note = "Вождение" }
        };

        public Task<List<Lesson>> GetAllAsync() => Task.FromResult(_lessons);

        public Task<Lesson?> GetByIdAsync(int id) => Task.FromResult(_lessons.FirstOrDefault(l => l.Id == id));

        public Task<Lesson> CreateAsync(Lesson lesson)
        {
            lesson.Id = _lessons.Any() ? _lessons.Max(l => l.Id) + 1 : 1;
            _lessons.Add(lesson);
            return Task.FromResult(lesson);
        }

        public Task<Lesson?> UpdateAsync(int id, Lesson lesson)
        {
            var existing = _lessons.FirstOrDefault(l => l.Id == id);
            if (existing == null) return Task.FromResult<Lesson?>(null);
            existing.StudentId = lesson.StudentId;
            existing.InstructorId = lesson.InstructorId;
            existing.VehicleId = lesson.VehicleId;
            existing.StartTime = lesson.StartTime;
            existing.EndTime = lesson.EndTime;
            existing.Note = lesson.Note;
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var lesson = _lessons.FirstOrDefault(l => l.Id == id);
            if (lesson != null)
            {
                _lessons.Remove(lesson);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<bool> IsAvailableAsync(Lesson lesson)
        {
            // Проверка пересечений для инструктора и автомобиля
            var conflict = _lessons.Any(l => l.Id != lesson.Id &&
                ((l.InstructorId == lesson.InstructorId) || (l.VehicleId == lesson.VehicleId)) &&
                l.StartTime < lesson.EndTime && l.EndTime > lesson.StartTime);
            return Task.FromResult(!conflict);
        }
    }
}