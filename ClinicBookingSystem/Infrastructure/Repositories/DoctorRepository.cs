using System;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
    }
}
