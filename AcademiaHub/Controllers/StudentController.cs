using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Student;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetStudents()
        {
            IEnumerable<Student> students = await _unitOfWork.StudentRepository.GetAsync
                (
                  include: s => s.Include(s => s.Account)
                );
            List<StudentGetRequest> studentGetRequests = _mapper.Map<List<StudentGetRequest>>(students.ToList());
            return Ok(studentGetRequests); 
        }

        [HttpGet]
        [Route("[action]/{id:guid}")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetStudentById([FromRoute] Guid id)
        {
            Student? student = await _unitOfWork.StudentRepository.FirstOrDefaultAsync
                (
                   filter: s => s.Id == id,
                   include: students => students.Include(s => s.Account)
                );

            if(student == null)
            {
                return NotFound(new { Message = $"No student found with id: {id}"});
            }

            StudentGetRequest studentGetRequest = _mapper.Map<StudentGetRequest>(student);

            return Ok(studentGetRequest);
        }
    }
}
