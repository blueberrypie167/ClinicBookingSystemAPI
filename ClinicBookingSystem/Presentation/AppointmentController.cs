


using ClinicBookingSystem.Features.AppointmentService;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation
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
        [HttpPost("{timeslotId}")] 
        public async Task<IActionResult> BookAppointment(int timeslotId, string Notes)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _appointmentService.BookTimeslotAsync(timeslotId, Notes, userId);

            return Ok(result);
        }

        [Authorize]
        [HttpPost("{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _appointmentService.CancelAppointment(appointmentId, userId);

            return Ok(result);
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetAppointmentsByUsername(string username)
        {
            var result = await _appointmentService.GetAppointmentsByUsername(username);

            return Ok(result);
        }
    }
}
