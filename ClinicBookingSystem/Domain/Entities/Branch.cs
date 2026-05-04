using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Branch
    {
        [Key]
        public int branchId { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }
    }
}
