using System;

namespace AutoDrive.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int InstructorId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Note { get; set; }
    }
}