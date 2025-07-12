using MedicalModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalService
{
    public class MedicalDbContext : DbContext
    {
        public MedicalDbContext(DbContextOptions options) : base(options)
        {
        }

        protected MedicalDbContext()
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Diagnosis> Diagnoses { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-6RQDDU3\\DOREMONSQLSERVER;Initial Catalog=MedicaProjectDb;Integrated Security=True;Encrypt=False");
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .OwnsOne(p => p.DoctorInfo);
            modelBuilder.Entity<Person>()
                .Navigation(p => p.DoctorInfo)
                .IsRequired();

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.PatientAppointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(p => p.DoctorAppointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appointment>()
                .OwnsOne(a => a.BookingSchedule);
            modelBuilder.Entity<Appointment>()
                .Navigation(a => a.BookingSchedule)
                .IsRequired();

            modelBuilder.Entity<Log>()
                .HasKey(l => l.Id);
            modelBuilder.Entity<Log>()
                .Property(l => l.Id)
                .ValueGeneratedOnAdd();

            var roleConverter = new ValueConverter<List<PersonRole>, string>(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(c => (PersonRole)Enum.Parse(typeof(PersonRole), c)).ToList());
            // TODO: will learn about this later
            var roleComparer = new ValueComparer<List<PersonRole>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                );
            modelBuilder.Entity<Person>()
                .Property(p => p.Roles)
                .HasConversion(roleConverter, roleComparer);

            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
