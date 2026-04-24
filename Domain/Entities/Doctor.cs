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
        public Guid branchId { get; set; }
        public Guid specialtyId { get; set; }
        public bool Is_Active { get; set; }

    }
}
