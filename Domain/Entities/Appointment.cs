using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Appointment
    {
        [Key]
        public Guid appointmentId { get; set; }

        public Guid timeSlotId { get; set; }
        public Guid patientUserId { get; set; }

        public enum Status {
            Confirmed,
            Cancelled
        }

        public string? Notes { get; set; }

        [Required]
        public TimestampAttribute Created_At { get; set; }

    }
}
