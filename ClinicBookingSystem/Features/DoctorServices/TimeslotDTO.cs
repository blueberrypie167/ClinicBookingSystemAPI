using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBookingSystem.Features.DoctorServices
{
    public class TimeslotDTO
    {
        public int timeslotId { get; set; }
        public DateTime Starts_At { get; set; }
        public TimeslotStatus CurrentStatus { get; set; }
        public int Duration { get; set; }
    }
}
