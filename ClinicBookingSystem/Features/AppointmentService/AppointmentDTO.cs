using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClinicBookingSystem.Features.AppointmentService
{
    public class AppointmentDTO
    {
        public int appointmentId { get; set; }

        public Guid patientUserId { get; set; }

        public string patientName { get; set; }

        public Guid doctorId { get; set; }

        public string doctorName { get; set; }

        public AppointmentStatus appointmentStatus { get; set; }

        public string? Notes { get; set; }

        public DateTime Created_At { get; set; }

    }
}
