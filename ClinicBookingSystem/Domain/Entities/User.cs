using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    // User entity, UserRole here is strictly for Authorization purposes
    public enum UserRole
    {
        Patient,
        Admin,
        Doctor
    }
    public class User
    {
        [Key]
        public Guid userId { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public UserRole userRole { get; set; }

        public Doctor? Doctor { get; set; }

    }
}
