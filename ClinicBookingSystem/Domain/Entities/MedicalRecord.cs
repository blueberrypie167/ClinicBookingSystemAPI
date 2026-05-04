using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class MedicalRecord
    {
        [Key]
        public int medicalRecordId { get; set; }

        public Guid patientId { get; set; }

        public int appointmentId { get; set; }
        
        public string? Summary { get; set; }

        public DateTime Recorded_At { get; set; }
    }
}
