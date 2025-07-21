using AcademiaHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _userService.GetAllUsersWithRolesAsync());
        }
    }
}
