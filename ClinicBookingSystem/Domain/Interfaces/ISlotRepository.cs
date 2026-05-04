using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ISlotRepository
    {
        public Task<Timeslot> CreateTimeslotAsync(Timeslot slot);

        public Task<Timeslot> GetTimeslotAsync(int timeslotId);

        public Task<List<Timeslot>> GetTimeslotsByDoctorIdAsync(Guid doctorId);
    }
}
