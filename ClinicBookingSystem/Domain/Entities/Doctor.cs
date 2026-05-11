using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public enum Specialty
    {
        GeneralPractitioner,
        Pediatrics,
        Dermatology,
        Cardiology,
        Gynecology,
        Orthopedics
    }
    public class Doctor
    {
        [Key]
        public Guid doctorId { get; set; }
        public Guid userId { get; set; }
        public Specialty Specialty { get; set; }
        public string? Username { get; set; }
        public bool Is_Active { get; set; }
        public User? User { get; set; }

        public ICollection<Timeslot>? Timeslots { get; set; }

        protected Doctor() { }

        public Doctor(Guid userid, Specialty specialty, string username)
        {
            userId = userid;
            doctorId = Guid.NewGuid();
            Is_Active = true;
            Specialty = specialty;
            Username = username;
        }

    }
}
