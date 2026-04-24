using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Specialty
    {
        public Guid specialtyId { get; set; }

        [Required]
        public string specialtyName { get; set; }
    }
}
