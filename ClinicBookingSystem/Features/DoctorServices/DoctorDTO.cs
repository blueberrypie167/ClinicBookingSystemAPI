using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicBookingSystem.Features.DoctorServices
{
    public class DoctorDTO
    {
        public Guid doctorId { get; set; }
        public int specialtyId { get; set; }
        public bool Is_Active { get; set; }
        public string? Username { get; set; }
    }
}
