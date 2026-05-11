using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ClinicBookingSystem.Features.SharedDtos
{
    public class PaginatedDTO
    {
        [Required]
        public required string? Username { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }

    }
}
