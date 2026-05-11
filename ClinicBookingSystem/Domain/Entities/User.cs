using Microsoft.AspNetCore.Identity;
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

        // needed for EF core
        protected User() { }

        public bool IsDoctor() => userRole is UserRole.Doctor;

        // AuthService concerns
        public User(string username, string passwordHash, UserRole role)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password cannot be empty", nameof(passwordHash));

            userId = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            userRole = role;
        }
        public bool VerifyPassword(string password, IPasswordHasher<User> hasher)
        {
            var verificationResult = hasher.VerifyHashedPassword(this, PasswordHash, password);
            return verificationResult != PasswordVerificationResult.Failed;
        }
        // Doctor role concerns

        public void PromoteToDoctor()
        {
            if (userRole == UserRole.Doctor)
            {
                throw new Exception("User is already a doctor");
            }

            userRole = UserRole.Doctor;
        }

    }
}
