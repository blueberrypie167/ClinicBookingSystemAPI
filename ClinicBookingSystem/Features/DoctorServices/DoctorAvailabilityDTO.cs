
namespace ClinicBookingSystem.Features.DoctorServices
{
    public class DoctorAvailabilityDTO
    {
        public Guid doctorId { get; set; }
        public string? username { get; set; }
        public int specialtyId { get; set; }
        public List<TimeslotDTO> timeslots { get; set; } = new();
    }
}