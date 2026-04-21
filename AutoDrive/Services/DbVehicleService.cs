using AutoDrive.Data;
using AutoDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Services
{
    public class DbVehicleService : IMockVehicleService
    {
        private readonly AppDbContext _context;

        public DbVehicleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle)
        {
            var existing = await _context.Vehicles.FindAsync(id);
            if (existing == null) return null;

            existing.Brand = vehicle.Brand;
            existing.Model = vehicle.Model;
            existing.LicensePlate = vehicle.LicensePlate;
            existing.Year = vehicle.Year;
            existing.InsuranceExpiry = vehicle.InsuranceExpiry;
            existing.InspectionExpiry = vehicle.InspectionExpiry;
            existing.MaintenanceDue = vehicle.MaintenanceDue;
            existing.Status = vehicle.Status;
            existing.InstructorId = vehicle.InstructorId;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null) return false;

            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}