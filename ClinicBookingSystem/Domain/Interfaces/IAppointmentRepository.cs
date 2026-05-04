using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        public Task<Appointment> CreateAppointmentAsync(Appointment appointment);

        public Task<Appointment?> GetAppointmentAsync(int appointmentId);

        public Task<List<Appointment?>> GetAllAppointmentsAsync(string username);
    }
}
