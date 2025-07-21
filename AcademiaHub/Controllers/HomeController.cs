using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public IActionResult GetList()
        {
            List<string> strings = new List<string>()
            {
                "Ahmed", "Omar", "Mohamed"
            };

            return Ok(strings);
        }
    }
}
