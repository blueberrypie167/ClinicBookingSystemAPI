using System;
using System.Collections.Generic;
using System.Text;

namespace ClinicBookingSystem.Features.Authentication
{
    public class AuthResultDTO
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public string? Role { get; set; }
    }
}
