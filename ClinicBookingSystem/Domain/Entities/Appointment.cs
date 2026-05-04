using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public enum AppointmentStatus
    {
        Confirmed,
        Cancelled
    }
    public class Appointment
    {
        [Key]
        public int appointmentId { get; set; }
        public required int timeslotId { get; set; }
        public Guid patientUserId { get; set; }

        public AppointmentStatus appointmentStatus { get; set; }

        public string? Notes { get; set; }

        public DateTime Created_At { get; set; }

        public Timeslot? Timeslot { get; set; }
        public void MarkAsCancelled()
        {
            appointmentStatus = AppointmentStatus.Cancelled;
        }

    }
}
