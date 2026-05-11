using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public class userDbContext : DbContext
    {
        public userDbContext() 
        {
        }

        public userDbContext(DbContextOptions<userDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Timeslot> timeSlots { get; set; }
        public DbSet<MedicalRecord> medicalRecords { get; set; }
        public DbSet<User> users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User -> Doctor (one-to-one)
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.userId);

            // Appointment -> timeslot
            modelBuilder.Entity<Appointment>()
                .HasOne(d => d.Timeslot)
                .WithOne(u => u.Appointment)
                .HasForeignKey<Appointment>(d => d.timeslotId);

            // Doctor -> Timeslots (one-to-many)
            modelBuilder.Entity<Timeslot>()
                .HasOne(ts => ts.Doctor)
                .WithMany(d => d.Timeslots)
                .HasForeignKey(ts => ts.doctorId);

            // Ensure a doctor cannot have two timeslots starting at the exact same time
            modelBuilder.Entity<Timeslot>()
                .HasIndex(t => new { t.doctorId, t.Starts_At })
                .IsUnique();

            // Appointment -> User (Patient)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.patientUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Appointment -> Doctor
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany()
                .HasForeignKey(a => a.doctorId)
                .OnDelete(DeleteBehavior.Restrict); 
        }

    }
}
