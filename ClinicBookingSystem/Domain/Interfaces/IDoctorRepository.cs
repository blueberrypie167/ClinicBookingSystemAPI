using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetDoctorByUserIdAsync(Guid userId);
        Task<Doctor> CreateNewDoctor(Doctor newDoctor);
        Task<string?> GetUsernameByDoctorIdAsync(Guid doctorId);
    }
}
