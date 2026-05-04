using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Infrastructure.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly userDbContext _context;

        public AppointmentRepository(userDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
        {
            await _context.appointments.AddAsync(appointment);
            

            return appointment;
        }

        public async Task<Appointment?> GetAppointmentAsync(int appointmentId)
        {
            var result = await _context.appointments.FindAsync(appointmentId);
            return result;
        }

        public async Task<List<Appointment?>> GetAllAppointmentsAsync(string username)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Username == username);
            return await _context.appointments.Where(u => u.patientUserId == user.userId).ToListAsync();
        }
    }
}
