using System.Collections.Generic;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public interface IMockVehicleService
    {
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle?> GetByIdAsync(int id);
        Task<Vehicle> CreateAsync(Vehicle vehicle);
        Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle);
        Task<bool> DeleteAsync(int id);
    }
}