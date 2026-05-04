using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Common.DTOs
{
    public class DoctorDTO
    {
        public Guid userId { get; set; }
        public int branchId { get; set; }
        public int specialtyId { get; set; }
        public bool Is_Active { get; set; }
    }
}
