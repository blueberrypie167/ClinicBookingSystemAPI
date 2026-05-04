using Microsoft.AspNetCore.Mvc;
using Features.DoctorServices;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Presentation
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

        [Authorize (Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDoctor([FromBody] string username)
        {
            // an admin can come in, create a new doctor entity, then link it to the userid
            var result = await _doctorService.CreateDoctor(username);

            return Ok(result);
        }

        
    }
}
