namespace ClinicBookingSystem.Features.AppointmentService
{
    public class CreateAppointmentDTO
    {
        public required int timeslotId { get; set; }

        public string? Notes { get; set; }
    }
}
