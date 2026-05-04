using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public enum TimeslotStatus
    {
        Open,
        Booked,
        Cancelled
    }
    public class Timeslot
    {       
        [Key]
        public int timeslotId { get; set; }
        public Guid doctorId { get; set; }

        public DateTime Starts_At { get; set; }

        public int Duration { get; set; }

        public TimeslotStatus CurrentStatus { get; set; }

        public bool IsAvailable() => CurrentStatus is TimeslotStatus.Open;

        public void MarkAsBooked()
        {
            if (!IsAvailable())
                throw new InvalidOperationException("Timeslot is no longer available.");

            CurrentStatus = TimeslotStatus.Booked;
        }

        public void MarkAsCancelled()
        {
            CurrentStatus = TimeslotStatus.Cancelled;
        }

        public void MarkAsAvailable()
        {
            CurrentStatus = TimeslotStatus.Open;
        }

        public Doctor? Doctor { get; set; }

        public Appointment? Appointment { get; set; }
    }
}
