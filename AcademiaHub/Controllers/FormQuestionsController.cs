using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Form_Questions;
using AcademiaHub.Models.Dto.QuestionBank;
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
    public class FormQuestionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FormQuestionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddQuestionsToForm([FromBody] List<FormQuestionsAddRequest> formQuestionsAddRequest)
        {
            if (!formQuestionsAddRequest.Any())
            {
                return BadRequest("No questions to add");
            }

            int formId = formQuestionsAddRequest[0].FormId;

            QuestionsForm? form =
                await _unitOfWork.QuestionsFormRepository.GetByIdAsync(formId);

            if (form == null)
            {
                return NotFound(new { Message = "Form id not exist" });
            }

            List<Form_Questions> form_Questions = [];
            List<QuestionBank> ClassroomQuestionBank =
                await _unitOfWork.QuestionBankRepository.GetAsync
                (
                    filter: q => q.ClassroomId == form.ClassroomId
                );
            foreach (var formQuestion in formQuestionsAddRequest)
            {
                QuestionBank? question =
                    ClassroomQuestionBank.
                    FirstOrDefault(q => q.QuestionId == formQuestion.QuestionId);

                if (formQuestion.FormId != formId || question == null)
                {
                    return BadRequest(new { Message = "Wrong data uploaded" });
                }

                form_Questions.Add(_mapper.Map<Form_Questions>(formQuestion));
            }

            await _unitOfWork.FormQuestionsRepository.AddRangeAsync(form_Questions);

            form.Active = true;
            _unitOfWork.QuestionsFormRepository.Update(form);

            await _unitOfWork.SaveChangesAsync();

            return Ok(new { Message = "Questions added to the form successfully" });
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        // Action for teacher
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetFormQuestionsByFormId([FromRoute] int id)
        {
            IEnumerable<Form_Questions> form_Questions =
                await _unitOfWork.FormQuestionsRepository.GetAsync
                (
                    filter: q => q.FormId == id,
                    include: q => q.Include(q => q.Question).ThenInclude(q => q!.Answers).
                                    Include(q => q.Question).ThenInclude(q => q!.Classroom).
                                    Include(q => q.Question).ThenInclude(q => q!.Difficulty).
                                    Include(q => q.Question).ThenInclude(q => q!.QuestionType)
                );

            List<QuestionsGetRequest> questionsGetRequests =
                _mapper.Map<List<QuestionsGetRequest>>(form_Questions.Select(q => q.Question).ToList());

            return Ok(questionsGetRequests);
        }

        [HttpGet]
        [Route("[action]/{studentId:guid}/{formId:int}")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Student,Teacher")]
        public async Task<IActionResult> GetFormQuestionsForStudentByFormAndStudentId([FromRoute] Guid studentId, [FromRoute] int formId)
        {
            IEnumerable<FormStudentAnswers> formStudentAnswers =
                await _unitOfWork.FormStudentAnswerRepository.GetAsync
                (
                    filter: f => f.StudentId == studentId && f.FormId == formId,
                    include: f => f.Include(f => f.Question).ThenInclude(q => q!.Answers).
                                    Include(f => f.Question).ThenInclude(q => q!.QuestionType)
                );
            List<FormQuestionsGetRequest> formQuestionsGetRequests = new List<FormQuestionsGetRequest>();
            if (formStudentAnswers.Any())
            {
                formQuestionsGetRequests = 
                    _mapper.Map<List<FormQuestionsGetRequest>>(formStudentAnswers);

                return Ok(formQuestionsGetRequests);
            }

            IEnumerable<Form_Questions> form_Questions =
                await _unitOfWork.FormQuestionsRepository.GetAsync
                (
                    filter: q => q.FormId == formId,
                    include: q => q.Include(q => q.Question).ThenInclude(q => q!.Answers).
                                    Include(q => q.Question).ThenInclude(q => q!.QuestionType)
                );

            formQuestionsGetRequests =
                _mapper.Map<List<FormQuestionsGetRequest>>(form_Questions);

            return Ok(formQuestionsGetRequests);
        }

    }
}
