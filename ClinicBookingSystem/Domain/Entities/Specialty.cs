using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Specialty
    {
        [Key]
        public int specialtyId { get; set; }

        [Required]
        public required string specialtyName { get; set; }
    }
}
