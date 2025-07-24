using AcademiaHub.CustomValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        public MaterialController()
        {
            
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        public IActionResult AddMaterial()
        {
            return Ok("Not Imolemented");
        }

        [HttpPut]
        [Route("[action]")]
        [ValidationModel]
        public IActionResult EditMaterial()
        {
            return Ok("Not Imolemented");
        }

        [HttpDelete]
        [Route("[action]")]
        [ValidationModel]
        public IActionResult DeleteMaterial()
        {
            return Ok("Not Imolemented");
        }
    }
}
