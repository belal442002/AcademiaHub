using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Student_QuestionsForm;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Student_QuestionsFormController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Student_QuestionsFormController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]/{studentId:guid}/{formId:int}")]
        [ValidationModel]
        public async Task<IActionResult> GetStudentGrade([FromRoute]Guid studentId, [FromRoute]int formId)
        {
            Student_QuestionsForm? student_QuestionsForm =
                await _unitOfWork.Student_QuestionsFormRepository.FirstOrDefaultAsync
                (
                    filter: f => f.QuestionsFormId == formId && f.StudentId == studentId,
                    include: f => f.Include(f => f.QuestionsForm).ThenInclude(f => f!.FormType).
                                    Include(f => f.QuestionsForm).ThenInclude(f => f!.FormDetailsId).
                                    Include(f => f.Studnet)
                );

            if(student_QuestionsForm != null)
            {
                return NotFound(new {Message = $"No Grade found for student with id: {studentId}," +
                    $" and form with id: {formId}"});
            }

            Student_QuestionsFormGetRequest student_QuestionsFormGetRequest =
                _mapper.Map<Student_QuestionsFormGetRequest>(student_QuestionsForm);

            return Ok(student_QuestionsFormGetRequest);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        public async Task<IActionResult> GetStudentGradesByClassroomId([FromRoute] int id)
        {
            Classroom? classroom = 
                await _unitOfWork.ClassroomRepository.FirstOrDefaultAsync
                (
                    filter: c => c.Id == id,
                    include: c => c.Include(c => c.Forms)
                );

            if (classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {id}"});
            }
            List<int> formsIds = classroom!.Forms!.Select(f => f.Id).ToList();

            List<Student_QuestionsForm> student_QuestionsForms =
                await _unitOfWork.Student_QuestionsFormRepository.GetAsync
                (
                    filter: f => formsIds.Contains(f.QuestionsFormId),
                    include: f => f.Include(f => f.QuestionsForm).ThenInclude(f => f!.FormType).
                                    Include(f => f.QuestionsForm).ThenInclude(f => f!.FormDetailsId).
                                    Include(f => f.Studnet)
                );

            List<Student_QuestionsFormGetRequest> student_QuestionsFormGetRequests =
                _mapper.Map<List<Student_QuestionsFormGetRequest>>(student_QuestionsForms);

            return Ok(student_QuestionsFormGetRequests);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        public async Task<IActionResult> AddGradeToStudent([FromBody] Student_QuestionsFormAddRequest student_QuestionsFormAddRequest)
        {
            Student? student =
                await _unitOfWork.StudentRepository.GetByIdAsync(student_QuestionsFormAddRequest.StudentId);
            if (student == null)
            {
                return NotFound(new { Message = $"No student found with id{student_QuestionsFormAddRequest.StudentId}"});
            }

            QuestionsForm? form =
                await _unitOfWork.QuestionsFormRepository.GetByIdAsync(student_QuestionsFormAddRequest.QuestionsFormId);
            if (form == null)
            {
                return NotFound(new { Message = $"No form found with id: {student_QuestionsFormAddRequest.QuestionsFormId}" });
            }

            Student_QuestionsForm? student_QuestionsForm =
                await _unitOfWork.Student_QuestionsFormRepository.
                GetByIdAsync(student_QuestionsFormAddRequest.StudentId, student_QuestionsFormAddRequest.QuestionsFormId);
            if(student_QuestionsForm != null)
            {
                return BadRequest(new { Message = "Student has already grade in this form"});
            }

            student_QuestionsForm =
                _mapper.Map<Student_QuestionsForm>(student_QuestionsFormAddRequest);

            await _unitOfWork.Student_QuestionsFormRepository.AddAsync(student_QuestionsForm);
            await _unitOfWork.SaveChangesAsync();

            Student_QuestionsFormGetRequest student_QuestionsFormGetRequest =
                _mapper.Map<Student_QuestionsFormGetRequest>(student_QuestionsForm);

            return CreatedAtAction(nameof(GetStudentGrade), new
            {
                studentId = student_QuestionsFormAddRequest.StudentId,
                formId = student_QuestionsFormAddRequest.QuestionsFormId
            },
            student_QuestionsFormGetRequest);
        }

        // Update
        [HttpPut]
        [Route("[action]")]
        [ValidationModel]
        public async Task<IActionResult> UpdateStudentGrade()
        {
            await Task.Delay(100);
            return Ok("Not implemented yet");
        }
    }
}
