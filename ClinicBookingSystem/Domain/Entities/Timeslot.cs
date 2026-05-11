using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text;

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
        // Identity

        [Key]
        public int timeslotId { get; set; }

        public Guid doctorId { get; set; }

        public TimeslotStatus CurrentStatus { get; set; }

        public DateTime Starts_At { get; set; }

        public int Duration { get; set; }

        public DateTime EndsAt => Starts_At.AddMinutes(Duration);

        // concurrency token
        [Timestamp] 
        public byte[]? RowVersion { get; set; }


        // Relationships
        public Doctor? Doctor { get; set; }

        public Appointment? Appointment { get; set; }


        // Functions
        protected Timeslot() { }

        public Timeslot(Guid doctorid, DateTime startsAt, int duration)
        {
            if (duration <= 0)
                throw new InvalidInputException("Duration must be greater than zero.");

            if (startsAt < DateTime.UtcNow)
                throw new InvalidInputException("Timeslot cannot start in the past.");

            doctorId = doctorid;
            Starts_At = startsAt;
            Duration = duration;
            CurrentStatus = TimeslotStatus.Open;
        }

        public bool IsAvailable() => CurrentStatus is TimeslotStatus.Open;

        public void MarkAsBooked()
        {
            if (!IsAvailable())
                throw new InvalidInputException("Timeslot is no longer available.");

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

        // time overlap check, returns false if a new timeslot conflicts with an existing one
        public bool OverlapsWith(DateTime otherStart, int otherDuration)
        {
            var otherEnd = otherStart.AddMinutes(otherDuration);
            return Starts_At < otherEnd && EndsAt > otherStart;
        }
    }
}
