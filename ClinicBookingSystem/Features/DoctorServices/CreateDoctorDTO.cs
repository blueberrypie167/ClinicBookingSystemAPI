using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem.Features.DoctorServices
{
    public class CreateDoctorDTO
    {
        public required string Username { get; set; }
        public required Specialty Specialty { get; set; }
    }
}
