using ClinicBookingSystem.Features.DoctorServices;
using ClinicBookingSystem.Features.SharedDtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicBookingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        
        private readonly DoctorService _doctorService;
        
        public DoctorController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [Authorize(Roles = "Doctor")]
        [HttpPost("Create-Timeslot")]
        public async Task<IActionResult> CreateTimeslot(CreateTimeslotDTO timeslotDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Missing or invalid user id claim.");
            }

            var result = await _doctorService.CreateTimeslotAsync(timeslotDto, userId);
            return CreatedAtAction(nameof(CreateTimeslot), result);
        }

        [Authorize]
        [HttpPost("View-Timeslots/")]
        public async Task<IActionResult> ViewTimeslotsByDoctor([FromBody] PaginatedDTO paginatedDTO)
        {
            var result = await _doctorService.GetAllTimeslots(paginatedDTO);

            return Ok(result);
        }

        [HttpGet("View-Doctors")]
        public async Task<IActionResult> ViewAllDoctors()
        {
            var result = await _doctorService.ViewAllDoctors();

            return Ok(result);
        }

        [HttpGet("View-Doctors-By-Specialty/{specialty}")]
        public async Task<IActionResult> ViewDoctorsBySpecialty(Specialty specialty)
        {
            var result = await _doctorService.ViewDoctorsBySpecialty(specialty);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{doctorId:guid}/availability")]
        public async Task<IActionResult> GetDoctorAvailability(Guid doctorId)
        {
            var result = await _doctorService.GetDoctorAvailabilityAsync(doctorId);
            return Ok(result);
        }
    }
}
