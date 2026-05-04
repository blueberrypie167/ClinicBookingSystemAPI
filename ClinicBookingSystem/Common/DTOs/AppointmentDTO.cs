using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTOs
{
    public class AppointmentDTO
    {
        [Key]
        public int appointmentId { get; set; }

        public Guid patientUserId { get; set; }

        public AppointmentStatus appointmentStatus { get; set; }

        public string? Notes { get; set; }

        public DateTime Created_At { get; set; }

    }
}
