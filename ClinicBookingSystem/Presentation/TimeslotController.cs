using Common.DTOs;
using Features.DoctorServices;
using Features.Slot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeslotController : ControllerBase
    {
        private readonly SlotService _slotService;

        public TimeslotController(SlotService slotService)
        {
            _slotService = slotService;
        }

        [Authorize (Roles = "Doctor")]
        [HttpPost("create-timeslot")]
        public async Task<IActionResult> CreateTimeslot([FromBody] TimeslotDTO timeslotDto)
        {
            // query the current doctor's entity, using the authorized logged in jwt token, then insert doctorid from there

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _slotService.CreateSlotAsync(timeslotDto, userId);
            return CreatedAtAction(nameof(CreateTimeslot), result);
        }

        [HttpGet("view-timeslots/{doctorName}")]
        public async Task<IActionResult> ViewTimeslotsByDoctor(string doctorName)
        {
            var result = await _slotService.GetAllTimeslots(doctorName);

            return Ok(result);
        }

    }
}
