using Domain.Entities;
using AutoMapper;
using Common.DTOs;

namespace Common
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<Timeslot, TimeslotDTO>().ReverseMap();
            CreateMap<Doctor, DoctorDTO>().ReverseMap();
            CreateMap<Appointment, AppointmentDTO>().ReverseMap();
            
        }
    }
}
