using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.FormType;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FormTypeController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FormTypeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetFormTypes()
        {
            IEnumerable<FormType> formTypes =
                await _unitOfWork.FormTypeRepository.GetAllAsync();

            List<FormTypeGetRequest> formTypeGetRequests =
                _mapper.Map<List<FormTypeGetRequest>>(formTypes.ToList());

            return Ok(formTypeGetRequests);
        }
    }
}
