using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class MedicalRecord
    {
        public Guid medicalRecordId { get; set; }

        public Guid patientId { get; set; }

        public Guid appointmentId { get; set; }
        
        public string? Summary { get; set; }

        public TimestampAttribute Recorded_At { get; set; }
    }
}
