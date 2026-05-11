using ClinicBookingSystem.Features.DoctorServices;
using ClinicBookingSystem.Features.SharedDtos;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISlotRepository
    {
        public Task<Timeslot> CreateTimeslotAsync(Timeslot slot);

        public Task<Timeslot> GetTimeslotAsync(int timeslotId);

        public Task<List<Timeslot>> GetTimeslotsByDoctorIdAsync(Guid doctorId);

        public Task<PagedResult<Timeslot>> GetTimeslotsByDoctorNameAsync(PaginatedDTO paginatedDTO);
        public Task<bool> HasOverlappingSlotAsync(
            Guid doctorId, CreateTimeslotDTO slotDTO, int? excludeId = null);
    }
}
