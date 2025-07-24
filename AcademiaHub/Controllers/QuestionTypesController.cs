using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.QuestionType;
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
    public class QuestionTypesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionTypesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetQuestionTypes()
        {
            IEnumerable<QuestionType> questionTypes =
                await _unitOfWork.QuestionTypeRepository.GetAllAsync();

            List<QuestionTypeGetRequest> questionTypeGetRequests =
                _mapper.Map<List<QuestionTypeGetRequest>>(questionTypes.ToList());

            return Ok(questionTypeGetRequests);
        }
    }
}
