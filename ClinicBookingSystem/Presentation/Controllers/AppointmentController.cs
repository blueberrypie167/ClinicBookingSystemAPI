using ClinicBookingSystem.Features.AppointmentService;
using ClinicBookingSystem.Features.DoctorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClinicBookingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [Authorize]
        [HttpPost("Book-Appointment")] 
        public async Task<IActionResult> BookAppointment(CreateAppointmentDTO timeslotDto)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Missing or invalid user id claim");
            }

            var result = await _appointmentService.BookTimeslotAsync(timeslotDto, userId);

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [Authorize]
        [HttpPut("Cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Missing or invalid user id claim");
            }
            var result = await _appointmentService.CancelAppointment(appointmentId, userId);

            return Ok(result);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("View-Patient-Appointments")]
        public async Task<IActionResult> GetAppointmentsForPatients()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Missing or invalid user id claim.");
            }
            var result = await _appointmentService.GetAppointmentsByUserId(userId);

            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("View-Doctor-Appointments")]
        public async Task<IActionResult> GetAppointmentsForDoctors()
        {
            // Extract the User Id from the active JWT Token
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized("Missing or invalid user id claim.");
            }

            var result = await _appointmentService.GetAppointmentsForDoctorAsync(userId);

            return Ok(result);
        }
    }
}
