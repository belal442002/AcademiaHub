using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassroomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassroomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetClassrooms()
        {
            IEnumerable<Classroom> classrooms = await _unitOfWork.ClassroomRepository.GetAsync
                (
                   include: classrooms => classrooms.Include(c => c.Subject)
                );

            List<ClassroomGetRequest> classroomGetRequests =
                _mapper.Map<List<ClassroomGetRequest>>(classrooms.ToList());

            return Ok(classroomGetRequests);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        public async Task<IActionResult> GetClassroomById([FromRoute] int id)
        {
            Classroom? classroom = await _unitOfWork.ClassroomRepository.FirstOrDefaultAsync
                (
                  filter: c => c.Id == id,
                  include: classrooms => classrooms.Include(c => c.Subject)
                );

            if(classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {id}"});
            }

            ClassroomGetRequest classroomGetRequest = _mapper.Map<ClassroomGetRequest>(classroom);

            return Ok(classroomGetRequest);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddClassroom([FromBody] ClassroomAddRequest classroomAddRequest)
        {
            Subject? subject =
                await _unitOfWork.SubjectRepository.GetByIdAsync(classroomAddRequest.SubjectId);

            if (subject == null)
                return NotFound(new { Message = $"No subject found with id: {classroomAddRequest.SubjectId}"});

            Classroom classroom = _mapper.Map<Classroom>(classroomAddRequest);

            await _unitOfWork.ClassroomRepository.AddAsync(classroom);
            await _unitOfWork.SaveChangesAsync();

            ClassroomGetRequest classroomGetRequest = _mapper.Map<ClassroomGetRequest>(classroom);

            return CreatedAtAction(nameof(GetClassroomById), new { id = classroomGetRequest.Id }, classroomGetRequest);
        }
    }
}
