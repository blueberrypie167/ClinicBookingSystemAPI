using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SlotRepository : ISlotRepository
    {
        private readonly userDbContext _context;

        public SlotRepository(userDbContext context)
        {
            _context = context;
        }

        public async Task<Timeslot> CreateTimeslotAsync(Timeslot slot)
        {
            var value = await _context.timeSlots.AddAsync(slot);
            await _context.SaveChangesAsync();
            return value.Entity;
        }

        public async Task<Timeslot?> GetTimeslotAsync(int timeslotId)
        {
            // use FindAsync, as timeslotid is the primary key
            var result = await _context.timeSlots.FindAsync(timeslotId);
            return result;
        }

        public Task<List<Timeslot>> GetTimeslotsByDoctorIdAsync(Guid doctorId)
        {
            return _context.timeSlots.Where(u => u.doctorId == doctorId).ToListAsync();
        }
    }
}
