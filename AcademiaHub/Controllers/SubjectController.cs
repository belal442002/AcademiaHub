using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Subject;
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
    public class SubjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetSubjects()
        {
            IEnumerable<Subject> subjects = await _unitOfWork.SubjectRepository.GetAllAsync();

            List<SubjectGetRequest> subjectGetRequests = 
                _mapper.Map<List<SubjectGetRequest>>(subjects.ToList());
            return Ok(subjectGetRequests);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetSubjectById(int id)
        {
            Subject? subject = await _unitOfWork.SubjectRepository.GetByIdAsync(id);

            if(subject == null)
            {
                return NotFound(new { Message = $"No subject found with id: {id}"});
            }

            SubjectGetRequest subjectGetRequest = _mapper.Map<SubjectGetRequest>(subject);

            return Ok(subjectGetRequest);
        }

        [HttpPost]
        [Route("[action]")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddSubject([FromBody] SubjectAddRequest subjectAddRequest)
        {
            Subject subject =  _mapper.Map<Subject>(subjectAddRequest);
            await _unitOfWork.SubjectRepository.AddAsync(subject);
            await _unitOfWork.SaveChangesAsync();
            SubjectGetRequest subjectGetRequest = _mapper.Map<SubjectGetRequest>(subject);
            return CreatedAtAction(nameof(GetSubjectById), new { id = subjectGetRequest.Id }, subjectGetRequest);
        }
    }
}
