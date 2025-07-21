using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
using AcademiaHub.Models.Dto.Teacher;
using AcademiaHub.Models.Dto.Teacher_Classroom;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherClassroomController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherClassroomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task <IActionResult> GetTeacherClassrooms()
        {
            IEnumerable<Teacher_Classroom> teacher_Classrooms =
                await _unitOfWork.TeacherClassroomRepository.GetAsync
                (
                    include: tc => tc.Include(tc => tc.Teacher).Include(tc => tc.Classroom)
                );

            List<Teacher_ClassroomGetRequest> teacher_ClassroomGetRequests =
                _mapper.Map<List<Teacher_ClassroomGetRequest>>(teacher_Classrooms.ToList());

            return Ok(teacher_ClassroomGetRequests);
        }

        [HttpGet]
        [Route("[action]/{teacherId:guid}/{classroomId:int}")]
        public async Task<IActionResult> GetTeacherClassroomById([FromRoute] Guid teacherId, [FromRoute] int classroomId)
        {
            Teacher_Classroom? teacher_Classroom =
                await _unitOfWork.TeacherClassroomRepository.FirstOrDefaultAsync
                (
                    filter: tc => tc.TeacherId == teacherId && tc.ClassroomId == classroomId,
                    include: tc => tc.Include(tc => tc.Classroom).Include(tc => tc.Teacher)
                );

            if(teacher_Classroom == null )
            {
                return NotFound(new { Message = $"No teacher with id: {teacherId} teach a classroom with id: {classroomId}"});
            }

            Teacher_ClassroomGetRequest teacher_ClassroomGetRequest =
                _mapper.Map<Teacher_ClassroomGetRequest>(teacher_Classroom);

            return Ok(teacher_ClassroomGetRequest);
        }

        [HttpGet]
        [Route("[action]/{id:guid}")]
        public async Task<IActionResult> GetClassroomByTeacherId(Guid id)
        {
            Teacher? teacher = await _unitOfWork.TeacherRepository.GetByIdAsync(id);

            if(teacher == null)
            {
                return NotFound(new { Message = $"No teacher found with id: {id}" });
            }

            Teacher_Classroom? teacher_Classroom = 
            await _unitOfWork.TeacherClassroomRepository.FirstOrDefaultAsync
                (
                   filter: tc => tc.TeacherId == id,
                   include: tc => tc.Include(tc => tc.Classroom).ThenInclude(c => c!.Subject)
                );
            if(teacher_Classroom == null )
            {
                return NotFound(new { Message = $"There is no classroom teached by teacher with id: {id}"});
            }

            Classroom classroom = teacher_Classroom.Classroom!;

            ClassroomGetRequest classroomGetRequest =
                _mapper.Map<ClassroomGetRequest>(classroom);

            return Ok(classroomGetRequest);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        public async Task<IActionResult> GetTeacherByClassroomId(int id)
        {
            Classroom? classroom = await _unitOfWork.ClassroomRepository.GetByIdAsync(id);

            if(classroom == null)
            {
                return NotFound(new { Messag = $"No classroom found with id: {id}" });
            }

            Teacher_Classroom? teacher_Classroom =
                await _unitOfWork.TeacherClassroomRepository.FirstOrDefaultAsync
                (
                    filter: tc => tc.ClassroomId == id,
                    include: tc => tc.Include(tc => tc.Teacher).ThenInclude(t => t!.Account)
                );

            if(teacher_Classroom == null )
            {
                return NotFound(new { Message = $"There is no teacher for classroom with id: {id}"});
            }

            Teacher teacher = teacher_Classroom.Teacher!;

            TeacherGetRequest teacherGetRequest =
                _mapper.Map<TeacherGetRequest>(teacher);

            return Ok(teacherGetRequest);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddTeacherToClassroom([FromBody] Teacher_ClassroomAddRequest teacher_ClassroomAddRequest)
        //{

        //}
    }
}
