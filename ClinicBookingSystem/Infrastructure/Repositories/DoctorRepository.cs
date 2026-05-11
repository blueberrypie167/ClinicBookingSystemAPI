using ClinicBookingSystem.Features.DoctorServices;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly userDbContext _context;

        public DoctorRepository(userDbContext context)
        {
            _context = context;
        }

        public async Task<Doctor?> GetDoctorByUserIdAsync(Guid userId)
        {
            return await _context.doctors.FirstOrDefaultAsync(d => d.userId == userId);
        }
        public async Task<string?> GetUsernameByDoctorIdAsync(Guid doctorId)
        {
            return await _context.doctors
                .Where(d => d.doctorId == doctorId)
                .Join(_context.users,
                      d => d.userId,
                      u => u.userId,
                      (d, u) => u.Username)
                .FirstOrDefaultAsync();
        }
        // SaveChangesAsync is removed, using Unit of Work pattern from now on.
        public async Task<Doctor> CreateNewDoctor(Doctor newDoctor)
        {
            await _context.doctors.AddAsync(newDoctor);
            
            return newDoctor;
        }

        public async Task<List<Doctor>> GetAllDoctorsAsync()
        {
            return await _context.doctors.Include(d => d.User).ToListAsync();
        }

        public async Task<List<Doctor>> GetDoctorsBySpecialtyAsync(Specialty specialty)
        {
            return await _context.doctors.Where(d => d.Specialty == specialty).ToListAsync();
        }

        public async Task<Doctor> GetDoctorByDoctorIdAsync(Guid doctorId)
        {
            return await _context.doctors.FindAsync(doctorId);
        }
    }
}
