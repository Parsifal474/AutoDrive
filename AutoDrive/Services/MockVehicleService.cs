using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoDrive.Models;

namespace AutoDrive.Services
{
    public class MockVehicleService : IMockVehicleService
    {
        private static List<Vehicle> _vehicles = new()
        {
            new Vehicle { Id = 1, Brand = "Lada", Model = "Granta", LicensePlate = "А123ВЕ", Year = 2020, InsuranceExpiry = new DateTime(2026, 12, 31), InspectionExpiry = new DateTime(2026, 10, 15), MaintenanceDue = new DateTime(2026, 8, 20), Status = "Активен", InstructorId = 1 },
            new Vehicle { Id = 2, Brand = "Hyundai", Model = "Solaris", LicensePlate = "В456КР", Year = 2021, InsuranceExpiry = new DateTime(2026, 11, 20), InspectionExpiry = new DateTime(2026, 9, 10), MaintenanceDue = new DateTime(2026, 7, 15), Status = "Активен", InstructorId = 2 },
            new Vehicle { Id = 3, Brand = "Kia", Model = "Rio", LicensePlate = "С789ТМ", Year = 2022, InsuranceExpiry = new DateTime(2026, 10, 5), InspectionExpiry = new DateTime(2026, 8, 1), MaintenanceDue = new DateTime(2026, 6, 10), Status = "Ремонт", InstructorId = null }
        };

        public Task<List<Vehicle>> GetAllAsync() => Task.FromResult(_vehicles);

        public Task<Vehicle?> GetByIdAsync(int id) => Task.FromResult(_vehicles.FirstOrDefault(v => v.Id == id));

        public Task<Vehicle> CreateAsync(Vehicle vehicle)
        {
            vehicle.Id = _vehicles.Any() ? _vehicles.Max(v => v.Id) + 1 : 1;
            _vehicles.Add(vehicle);
            return Task.FromResult(vehicle);
        }

        public Task<Vehicle?> UpdateAsync(int id, Vehicle vehicle)
        {
            var existing = _vehicles.FirstOrDefault(v => v.Id == id);
            if (existing == null) return Task.FromResult<Vehicle?>(null);
            existing.Brand = vehicle.Brand;
            existing.Model = vehicle.Model;
            existing.LicensePlate = vehicle.LicensePlate;
            existing.Year = vehicle.Year;
            existing.InsuranceExpiry = vehicle.InsuranceExpiry;
            existing.InspectionExpiry = vehicle.InspectionExpiry;
            existing.MaintenanceDue = vehicle.MaintenanceDue;
            existing.Status = vehicle.Status;
            existing.InstructorId = vehicle.InstructorId;
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var vehicle = _vehicles.FirstOrDefault(v => v.Id == id);
            if (vehicle != null)
            {
                _vehicles.Remove(vehicle);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}