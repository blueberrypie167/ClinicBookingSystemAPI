using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTOs
{
    public class TimeslotDTO
    {
        public int timeslotId { get; set; }
        public DateTime Starts_At { get; set; }

        public int Duration { get; set; }
    }
}
