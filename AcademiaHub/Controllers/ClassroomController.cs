using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
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
        [ValidationModel]
        //[Authorize(Roles = "Admin,Student,Teacher")]
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
        [ValidationModel]
        //[Authorize(Roles = "Admin,Student,Teacher")]
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
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
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

        //[HttpDelete]
        //[Route("[action]/{id:int}")]
        //public async Task<IActionResult> DeleteClassroom([FromRoute] int id)
        //{
        //    Classroom? classroom = await _unitOfWork.ClassroomRepository.GetByIdAsync(id);
        //    if(classroom == null)
        //    {
        //        return NotFound(new { Message = $"No classroom found with id: {id}"});
        //    }

        //    IEnumerable<Student_Classroom> student_Classrooms = 
        //    await _unitOfWork.StudentClassroomRepository.GetAsync
        //    (
        //        filter: sc => sc.ClassroomId == id
        //    );

        //    if(student_Classrooms.Any())
        //    {
        //        return BadRequest(new { Message = "There are students registered in this classroom" +
        //                                "Remove them first to delete"});
        //    }

        //    Teacher_Classroom? teacher_Classroom =
        //        await _unitOfWork.TeacherClassroomRepository.FirstOrDefaultAsync
        //        (
        //            filter: tc => tc.ClassroomId == id
        //        );

        //    if(teacher_Classroom != null)
        //    {
        //        return BadRequest(new
        //        {
        //            Message = "There is a teacher in this classroom" +
        //                                "Remove him first to delete"
        //        });
        //    }

        //    IEnumerable<QuestionBank> questions = 
        //        await _unitOfWork
        //}
    }
}
