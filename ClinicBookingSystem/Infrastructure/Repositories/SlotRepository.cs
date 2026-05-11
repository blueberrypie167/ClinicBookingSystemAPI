using ClinicBookingSystem.Features.DoctorServices;
using ClinicBookingSystem.Features.SharedDtos;
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
            return value.Entity;
        }
        
        public async Task<bool> HasOverlappingSlotAsync(
            Guid doctorId, CreateTimeslotDTO slotDto, int? excludeId = null)
        {
            var endsAt = slotDto.Starts_At.AddMinutes(slotDto.Duration);

            return await _context.timeSlots
                .Where(ts => ts.doctorId == doctorId
                    && ts.CurrentStatus != TimeslotStatus.Cancelled
                    && (excludeId == null || ts.timeslotId != excludeId)
                    && ts.Starts_At < endsAt
                    && ts.Starts_At.AddMinutes(ts.Duration) > slotDto.Starts_At)
                .AnyAsync();
        }

        public async Task<Timeslot> GetTimeslotAsync(int timeslotId)
        {
            return await _context.timeSlots.FindAsync(timeslotId);
        }

        public Task<List<Timeslot>> GetTimeslotsByDoctorIdAsync(Guid doctorId)
        {
            return _context.timeSlots.Where(u => u.doctorId == doctorId).ToListAsync();
        }

        public async Task<PagedResult<Timeslot>> GetTimeslotsByDoctorNameAsync(PaginatedDTO paginatedDTO)
        {
            if (string.IsNullOrWhiteSpace(paginatedDTO.Username))
            {
                throw new ArgumentException("Username is required.", nameof(paginatedDTO.Username));
            }

            var page = paginatedDTO.Page < 1 ? 1 : paginatedDTO.Page;
            var pageSize = paginatedDTO.PageSize < 1 ? 10 : paginatedDTO.PageSize;

            var query = _context.timeSlots
                .AsNoTracking()
                .Where(ts => ts.Doctor != null && ts.Doctor.Username == paginatedDTO.Username)
                .OrderBy(ts => ts.Starts_At);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Timeslot>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
