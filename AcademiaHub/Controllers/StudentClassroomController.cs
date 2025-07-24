using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
using AcademiaHub.Models.Dto.Student;
using AcademiaHub.Models.Dto.Student_Classroom;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentClassroomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public StudentClassroomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]/{studentId:guid}/{classroomId:int}")]
        [ValidationModel]
        public async Task<IActionResult> GetStudentClassroomById([FromRoute] Guid studentId, [FromRoute] int classroomId)
        {
            Student_Classroom? student_Classroom =
                await _unitOfWork.StudentClassroomRepository.FirstOrDefaultAsync
                (
                    filter: sc => (sc.StudentId == studentId && sc.ClassroomId == classroomId),
                    include: sc => sc.Include(sc => sc.Student).Include(sc => sc.Classroom)
                );

            if(student_Classroom == null)
            {
                return NotFound(new
                {
                    Message = $"No student in classroom with vallues studentId:" +
                $" {studentId}, classroomId: {classroomId}"
                });
            }

            Student_ClassroomGetRequest student_ClassroomGetRequest =
                _mapper.Map<Student_ClassroomGetRequest>(student_Classroom);

            return Ok(student_ClassroomGetRequest);
        }

        [HttpGet]
        [Route("[action]")]
        [ValidationModel]
        public async Task<IActionResult> GetStudentClassrooms()
        {
            IEnumerable<Student_Classroom> student_Classrooms =
                await _unitOfWork.StudentClassroomRepository.GetAsync
                (
                    include: sc => sc.Include(sc => sc.Student).Include(sc => sc.Classroom) 
                 );

            List<Student_ClassroomGetRequest> student_ClassroomGetRequests =
                _mapper.Map<List<Student_ClassroomGetRequest>>(student_Classrooms.ToList());

            return Ok(student_ClassroomGetRequests);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        public async Task<IActionResult> AddStudentToClassroom([FromBody] Student_ClassroomAddRequest student_ClassroomAddRequest)
        {
            Student? student =
                await _unitOfWork.StudentRepository.GetByIdAsync(student_ClassroomAddRequest.StudentId);

            if(student == null)
            {
                return NotFound(new { Message = $"No student found with id: {student_ClassroomAddRequest.StudentId}"});
            }

            Classroom? classroom =
                await _unitOfWork.ClassroomRepository.GetByIdAsync(student_ClassroomAddRequest.ClassroomId);

            if(classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {student_ClassroomAddRequest.ClassroomId}" });
            }

            Student_Classroom? is_StudClass_Exist =
                await _unitOfWork.StudentClassroomRepository.
                GetByIdAsync(student_ClassroomAddRequest.StudentId, student_ClassroomAddRequest.ClassroomId);
            
            if(is_StudClass_Exist != null)
            {
                return BadRequest(new { Message = "This student is already registered in this classroom"});
            }

            Student_Classroom student_Classroom = 
                _mapper.Map<Student_Classroom>(student_ClassroomAddRequest);

            await _unitOfWork.StudentClassroomRepository.AddAsync(student_Classroom);
            await _unitOfWork.SaveChangesAsync();

            Student_ClassroomGetRequest student_ClassroomGetRequest =
                _mapper.Map<Student_ClassroomGetRequest>(student_Classroom);

            return CreatedAtAction(nameof(GetStudentClassroomById),
                new
                {
                    studentId = student_ClassroomGetRequest.StudentId,
                    classroomId = student_ClassroomGetRequest.ClassroomId
                }
                , student_ClassroomGetRequest);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        public async Task<IActionResult> GetStudentsByClassroomId(int id)
        {
            Classroom? classroom =
                await _unitOfWork.ClassroomRepository.GetByIdAsync(id);

            if(classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {id}"});
            }

            IEnumerable<Student_Classroom> student_Classrooms =
                await _unitOfWork.StudentClassroomRepository.GetAsync
                (
                    filter: sc => sc.ClassroomId == id,
                    include: sc => sc.Include(sc => sc.Student).ThenInclude(s => s!.Account)
                 );

            if (!student_Classrooms.Any())
            {
                return NotFound(new { Message = $"This classroom has no student registered in it yet" });
            }

            List<Student> students =
                student_Classrooms.Select(sc => sc.Student).ToList()!;

            List<StudentGetRequest> studentGetRequests =
                _mapper.Map<List<StudentGetRequest>>(students);

            return Ok(studentGetRequests);
        }

        [HttpGet]
        [Route("[action]/{id:guid}")]
        [ValidationModel]
        public async Task<IActionResult> GetClassroomsByStudentId(Guid id)
        {
            Student? student = await _unitOfWork.StudentRepository.GetByIdAsync(id);

            if(student == null)
            {
                return NotFound(new { Message = $"No student found with id: {id}"});
            }

            IEnumerable<Student_Classroom> student_Classrooms =
                await _unitOfWork.StudentClassroomRepository.GetAsync
                (
                    filter: sc => sc.StudentId == id,
                    include: sc => sc.Include(sc => sc.Classroom).ThenInclude(c => c!.Subject)
                );

            if(!student_Classrooms.Any())
            {
                return NotFound(new { Message = $"This student has not registered in any classrooms yet"});
            }

            List<Classroom> classrooms = 
                student_Classrooms.Select(sc => sc.Classroom).ToList()!;

            List<ClassroomGetRequest> classroomGetRequests =
                _mapper.Map<List<ClassroomGetRequest>>(classrooms);

            return Ok(classroomGetRequests);    
        }



    }
}
