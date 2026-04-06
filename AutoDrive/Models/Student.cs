namespace AutoDrive.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Status { get; set; } = "Новый";
        public string? ContractNumber { get; set; }
    }
}