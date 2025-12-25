using MedicalCard1.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalCard1
{
    public class AppContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=databaseSqlite.db");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Role>().HasData(new[]
            {
                new Role { Id = 1, Name = "Супер-Администратор" },
                new Role { Id = 2, Name = "Администратор" },
                new Role { Id = 3, Name = "Врач" }
            });

            // Связь между пользователями и ролями
            builder.Entity<User>()
                   .HasOne(u => u.Role)
                   .WithMany(r => r.Users)
                   .HasForeignKey(u => u.RoleId);

            // Связь между пациентами и визитами
            builder.Entity<Appointment>()
                   .HasOne(a => a.Patient)
                   .WithMany(p => p.Appointments)
                   .HasForeignKey(a => a.PatientId);

            // Связь между визитоми и врачами
            builder.Entity<Appointment>()
                   .HasOne(a => a.Doctor)
                   .WithMany(u => u.Appointments)
                   .HasForeignKey(a => a.DoctorId);
            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Super Admin",
                LastName = "_",
                Login = "1234",
                Password = "1234",
                RoleId = 1
            });
            builder.Entity<User>().HasData(new User
            {
                Id = 2,
                Name = "Admin",
                LastName = "Adminovich",
                Login = "1a",
                Password = "1a",
                RoleId = 2
            });
            builder.Entity<User>().HasData(new User
            {
                Id = 3,
                Name = "Doctor",
                LastName = "Doctorovich",
                Login = "1d",
                Password = "1d",
                RoleId = 3
            });
            builder.Entity<Patient>().HasData(new Patient
            {
                Id = 4,
                Name = "Patient",
                LastName = "Patientovich",
                Passport = 123,
                Birth = new DateTime(2000, 1, 1)
            });
            builder.Entity<Appointment>().HasData(new Appointment
            {
                Id = 5,
                VisitData = new DateTime(2025, 12, 15),
                Complaints = "1",
                Anamnesis = "2",
                Diagnosis = "3",
                DoctorsOrder = "4",
                PatientId = 4,
                DoctorId = 3,
            });
            builder.Entity<Appointment>().HasData(new Appointment
            {
                Id = 6,
                VisitData = new DateTime(2025, 12, 16),
                Complaints = "1",
                Anamnesis = "2",
                Diagnosis = "3",
                DoctorsOrder = "4",
                PatientId = 4,
                DoctorId = 3,
            });
        }
    }
}
