using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Services
{
    public class DbLessonService : IMockLessonService
    {
        private readonly AppDbContext _context;

        public DbLessonService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Lesson>> GetAllAsync()
        {
            return await _context.Lessons.ToListAsync();
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons.FindAsync(id);
        }

        public async Task<Lesson> CreateAsync(Lesson lesson)
        {
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();
            return lesson;
        }

        public async Task<Lesson?> UpdateAsync(int id, Lesson lesson)
        {
            var existing = await _context.Lessons.FindAsync(id);
            if (existing == null) return null;

            existing.StudentId = lesson.StudentId;
            existing.InstructorId = lesson.InstructorId;
            existing.VehicleId = lesson.VehicleId;
            existing.StartTime = lesson.StartTime;
            existing.EndTime = lesson.EndTime;
            existing.Note = lesson.Note;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            if (lesson == null) return false;

            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsAvailableAsync(Lesson lesson)
        {
            // Проверка конфликтов: пересечение по времени с тем же инструктором или автомобилем
            bool conflict = await _context.Lessons.AnyAsync(l =>
                l.Id != lesson.Id &&
                (l.InstructorId == lesson.InstructorId || l.VehicleId == lesson.VehicleId) &&
                l.StartTime < lesson.EndTime && l.EndTime > lesson.StartTime);
            return !conflict;
        }
    }
}