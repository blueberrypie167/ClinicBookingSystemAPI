using ClinicBookingSystem.Features.DoctorServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly DoctorService _doctorService;

        public AdminController(DoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("test/admin-only")]
        public IActionResult AdminAuthenticated()
        {
            return Ok("You are Admin"); 
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create-Doctor")]
        public async Task<IActionResult> CreateDoctor(CreateDoctorDTO doctor)
        {
            // an admin can come in, create a new doctor entity, then link it to the userid
            var result = await _doctorService.CreateDoctor(doctor);

            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
