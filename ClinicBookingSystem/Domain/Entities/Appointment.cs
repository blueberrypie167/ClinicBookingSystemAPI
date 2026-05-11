using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static Azure.Core.HttpHeader;

namespace Domain.Entities
{
    public enum AppointmentStatus
    {
        Confirmed,
        Cancelled
    }
    public class Appointment
    {
        // Identity
        [Key]
        public int appointmentId { get; set; }
        public required int timeslotId { get; set; }
        public Guid patientUserId { get; set; }

        public Guid doctorId { get; set; }

        public AppointmentStatus appointmentStatus { get; set; }

        public string? Notes { get; set; }

        public DateTime Created_At { get; set; }


        // Relationships
        public Timeslot? Timeslot { get; set; }

        public Doctor? Doctor { get; set; }

        public User? User { get; set; }

        protected Appointment() { }
        public Appointment(int timeslotid, Guid patientid, Guid doctorid, string notes, string patientName, string doctorName)
        {
            timeslotId = timeslotid;
            patientUserId = patientid;
            doctorId = doctorid;
            Notes = notes;
            appointmentStatus = AppointmentStatus.Confirmed;
            Created_At = DateTime.UtcNow;
        }
        
        public void MarkAsCancelled()
        {
            appointmentStatus = AppointmentStatus.Cancelled;
        }
        public void MarkAsCancelled(Guid requestPatientId)
        {
            if (patientUserId != requestPatientId)
            {
                throw new ConflictException("You are not authorized to cancel this appointment");
            }

            if (appointmentStatus == AppointmentStatus.Cancelled)
            {
                throw new ConflictException("Appointment is already cancelled");
            }

            appointmentStatus = AppointmentStatus.Cancelled;
        }
    }
}
