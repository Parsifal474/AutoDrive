using System.Collections.Generic;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public interface IMockLessonService
    {
        Task<List<Lesson>> GetAllAsync();
        Task<Lesson?> GetByIdAsync(int id);
        Task<Lesson> CreateAsync(Lesson lesson);
        Task<Lesson?> UpdateAsync(int id, Lesson lesson);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsAvailableAsync(Lesson lesson); // проверка конфликтов
    }
}