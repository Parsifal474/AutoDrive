using AutoDrive.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoDrive.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальность телефона студента
            modelBuilder.Entity<Student>()
                .HasIndex(s => s.Phone)
                .IsUnique();

            // Настройка связей для Lesson
            modelBuilder.Entity<Lesson>()
                .HasOne<Student>()
                .WithMany()
                .HasForeignKey(l => l.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasOne<Instructor>()
                .WithMany()
                .HasForeignKey(l => l.InstructorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Lesson>()
                .HasOne<Vehicle>()
                .WithMany()
                .HasForeignKey(l => l.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь Vehicle → Instructor (один ко многим)
            modelBuilder.Entity<Vehicle>()
                .HasOne(v => v.Instructor)
                .WithMany(i => i.Vehicles)
                .HasForeignKey(v => v.InstructorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}