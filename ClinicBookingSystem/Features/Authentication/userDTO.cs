using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBookingSystem.Features.Authentication
{
    public class userDTO
    {
        public required string Username { get; set; } = string.Empty;

        public required string Password { get; set; } = string.Empty;

        
    }
}
