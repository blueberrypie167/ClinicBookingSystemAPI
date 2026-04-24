using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Timeslot
    {
        public Guid timeslotId { get; set; }
        public Guid doctorId { get; set; }

        public DateTime Starts_At { get; set; }

        public int Duration { get; set; }

        public enum Status
        {
            Open, 
            Booked, 
            Cancelled
        }
    }
}
