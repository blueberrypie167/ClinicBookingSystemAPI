using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem.Features.DoctorServices
{
    // used for timeslot input ONLY
    public class CreateTimeslotDTO
    {
        public required DateTime Starts_At { get; set; }
        public required int Duration { get; set; }
    }
}
