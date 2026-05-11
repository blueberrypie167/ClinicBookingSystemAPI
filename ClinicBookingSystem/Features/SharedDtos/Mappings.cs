using Domain.Entities;
using AutoMapper;
using ClinicBookingSystem.Features.AppointmentService;
using ClinicBookingSystem.Features.DoctorServices;

namespace ClinicBookingSystem.Features.SharedDtos
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Timeslot, TimeslotDTO>().ReverseMap();
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>()
                .ForMember(dest => dest.patientName, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.doctorName, opt => opt.MapFrom(src => src.Doctor.User.Username))
                .ReverseMap();

        }
    }
}
