using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Classroom;
using AcademiaHub.Models.Dto.Teacher;
using AcademiaHub.Models.Dto.Teacher_Classroom;
using AcademiaHub.Repositories;
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
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "Admin,Teacher")]
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
        //[Authorize(Roles = "Admin")]
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

        [HttpPost]
        [Route("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTeacherToClassroom([FromBody] Teacher_ClassroomAddRequest teacher_ClassroomAddRequest, [FromQuery] bool? replace = null)
        {
            Teacher? teacher =
                await _unitOfWork.TeacherRepository.
                GetByIdAsync(teacher_ClassroomAddRequest.TeacherId);

            if(teacher == null )
            {
                return NotFound(new { Message = $"No teacher found with id: {teacher_ClassroomAddRequest.TeacherId}"});
            }

            Classroom? classroom =
                await _unitOfWork.ClassroomRepository.
                GetByIdAsync(teacher_ClassroomAddRequest.ClassroomId);

            if(classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {teacher_ClassroomAddRequest.ClassroomId}"});
            }

            Teacher_Classroom? teacher_Classroom_ByTeacherId =
                await _unitOfWork.TeacherClassroomRepository.
                GetByIdAsync(teacher_ClassroomAddRequest.TeacherId);


            Teacher_Classroom? teacher_Classroom_ByClassroomId =
                await _unitOfWork.TeacherClassroomRepository.
                FirstOrDefaultAsync
                (filter: tc => tc.ClassroomId == teacher_ClassroomAddRequest.ClassroomId);

            if(teacher_Classroom_ByTeacherId == null &&
               teacher_Classroom_ByClassroomId == null)
            {
                Teacher_Classroom teacher_Classroom =
                    _mapper.Map<Teacher_Classroom>(teacher_ClassroomAddRequest);

                await _unitOfWork.TeacherClassroomRepository.AddAsync(teacher_Classroom);
                await _unitOfWork.SaveChangesAsync();

                Teacher_ClassroomGetRequest teacher_ClassroomGetRequest =
                    _mapper.Map<Teacher_ClassroomGetRequest>(teacher_Classroom);

                return CreatedAtAction(nameof(GetTeacherClassroomById),
                    new
                    {
                        teacherId = teacher_ClassroomGetRequest.TeacherId,
                        classroomId = teacher_ClassroomGetRequest.ClassroomId
                    },
                    teacher_ClassroomGetRequest);
            }

            else if(teacher_Classroom_ByTeacherId !=null &&
                    teacher_Classroom_ByClassroomId != null &&
                    teacher_Classroom_ByTeacherId!.TeacherId ==
                    teacher_Classroom_ByClassroomId!.TeacherId &&
                    teacher_Classroom_ByTeacherId!.ClassroomId ==
                    teacher_Classroom_ByClassroomId!.ClassroomId)
            {
                return BadRequest(new { Message = "This teacher already teaches this classroom" });
            }

            else if(teacher_Classroom_ByTeacherId != null &&
                    teacher_Classroom_ByClassroomId != null &&
                    teacher_Classroom_ByTeacherId!.TeacherId !=
                    teacher_Classroom_ByClassroomId!.TeacherId &&
                    teacher_Classroom_ByTeacherId!.ClassroomId !=
                    teacher_Classroom_ByClassroomId!.ClassroomId)
            {
                return BadRequest(new { Message = "This teacher is already assigned to another classroom" +
                                                  "and this classroom is teached by a teacher" });
            }

            else if(teacher_Classroom_ByTeacherId == null &&
                    teacher_Classroom_ByClassroomId != null &&
                    replace == true)
            {
                Teacher_Classroom teacher_Classroom =
                    _mapper.Map<Teacher_Classroom>(teacher_ClassroomAddRequest);

                _unitOfWork.TeacherClassroomRepository.Update(teacher_Classroom);
                await _unitOfWork.SaveChangesAsync();

                Teacher_ClassroomGetRequest teacher_ClassroomGetRequest =
                    _mapper.Map<Teacher_ClassroomGetRequest>(teacher_Classroom);

                return Ok(new { 
                      Message = $"Teacher with id: {teacher_Classroom_ByClassroomId.TeacherId}" +
                                $" replaced successfully with teacher with id: {teacher_ClassroomAddRequest.TeacherId}",
                      Teacher_Classroom = teacher_ClassroomGetRequest
                }); 
            }

            else if (teacher_Classroom_ByTeacherId == null &&
                    teacher_Classroom_ByClassroomId != null &&
                    replace == false)
            {
                return BadRequest(new { Message = "This classroom is already teached by " +
                    $"teacher with id: {teacher_Classroom_ByClassroomId.TeacherId}"});
            }

            else
            {
                return BadRequest(new { Message = "There is a problem with assigning" +
                                        " this teacher to this classroom"});
            }
        }

        [HttpDelete]
        [Route("[action]/{teacherId:guid}/{classroomId:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacherClassroom([FromRoute] Guid teacherId, [FromRoute] int classroomId)
        {
            Teacher_Classroom? teacher_Classroom =
                await _unitOfWork.TeacherClassroomRepository.FirstOrDefaultAsync
                (
                    filter: tc => tc.TeacherId == teacherId && tc.ClassroomId == classroomId
                );

            if(teacher_Classroom == null)
            {
                return NotFound(new { Message = "No Teacher_Classroom found" });
            }

             _unitOfWork.TeacherClassroomRepository.Delete(teacher_Classroom);
            await _unitOfWork.SaveChangesAsync();

            Teacher_ClassroomGetRequest teacher_ClassroomGetRequest =
                _mapper.Map<Teacher_ClassroomGetRequest>(teacher_Classroom);

            return NoContent();
        }

        [HttpDelete]
        [Route("[action]/{teacherId:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeacherClassroom([FromRoute] Guid teacherId)
        {
            Teacher_Classroom? teacher_Classroom =
                await _unitOfWork.TeacherClassroomRepository.GetByIdAsync(teacherId);

            if (teacher_Classroom == null)
            {
                return NotFound(new { Message = $"No Teacher found with id: {teacherId}" });
            }

            _unitOfWork.TeacherClassroomRepository.Delete(teacher_Classroom);
            await _unitOfWork.SaveChangesAsync();

            Teacher_ClassroomGetRequest teacher_ClassroomGetRequest =
                _mapper.Map<Teacher_ClassroomGetRequest>(teacher_Classroom);

            return NoContent();
        }
    }
}
