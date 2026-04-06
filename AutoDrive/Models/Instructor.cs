using System.Collections.Generic;

namespace AutoDrive.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public decimal HourlyRate { get; set; }                // Ставка за час
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}