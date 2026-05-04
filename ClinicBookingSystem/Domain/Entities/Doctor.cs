using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Doctor
    {
        [Key]
        public Guid doctorId { get; set; }
        public Guid userId { get; set; }
        public int branchId { get; set; }
        public int specialtyId { get; set; }
        public bool Is_Active { get; set; }
        public User? User { get; set; }

        public ICollection<Timeslot>? Timeslots { get; set; }

    }
}
