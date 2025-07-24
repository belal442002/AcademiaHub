using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.QuestionBank;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class QuestionBankController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionBankController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]/{id:guid}")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetQuestionById([FromRoute] Guid id)
        {
            QuestionBank? question =
                await _unitOfWork.QuestionBankRepository.FirstOrDefaultAsync
                (
                    filter: q => q.QuestionId == id,
                    include: q => q.Include(q => q.Answers).
                                    Include(q => q.Classroom).
                                    Include(q => q.Difficulty).
                                    Include(q => q.QuestionType)
                );

            if (question == null)
            {
                return NotFound(new { Message = $"No question found with id: {id}"});
            }

            QuestionsGetRequest questionGetRequest = 
                _mapper.Map<QuestionsGetRequest>(question);

            return Ok(questionGetRequest);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetQuestionsByClassroomId([FromRoute] int id)
        {
            Classroom? classroom = 
                await _unitOfWork.ClassroomRepository.GetByIdAsync(id);

            if(classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {id}"});
            }

            List<QuestionBank> questions =
                await _unitOfWork.QuestionBankRepository.GetAsync
                (
                    filter: q => q.ClassroomId == id,
                    include: q => q.Include(q => q.Answers).
                                    Include(q => q.Classroom).
                                    Include(q => q.Difficulty).
                                    Include(q => q.QuestionType)
                );


            List<QuestionsGetRequest> questionsGetRequest =
                _mapper.Map<List<QuestionsGetRequest>>(questions);

            return Ok(questionsGetRequest);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionsAddRequest questionAddRequest)
        {
            QuestionBank question = _mapper.Map<QuestionBank>(questionAddRequest);

            await _unitOfWork.QuestionBankRepository.AddAsync(question);
            await _unitOfWork.SaveChangesAsync();

            QuestionsGetRequest questionGetRequest = 
                _mapper.Map<QuestionsGetRequest>(question);

            return CreatedAtAction(nameof(GetQuestionById), 
                new
                {
                    id = questionGetRequest.QuestionId
                },
                questionGetRequest);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> AddQuestions([FromBody] List<QuestionsAddRequest> questionsAddRequest)
        {
            List<QuestionBank> questions =
                _mapper.Map<List<QuestionBank>>(questionsAddRequest);

            await _unitOfWork.QuestionBankRepository.AddRangeAsync(questions);
            await _unitOfWork.SaveChangesAsync();

            List<QuestionsGetRequest> questionsGetRequest =
                _mapper.Map<List<QuestionsGetRequest>>(questions);

            return Created(string.Empty, questionsGetRequest);
        }

        [HttpDelete]
        [Route("[action]/{id:guid}")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> DeleteQuestionById([FromRoute] Guid id)
        {
            QuestionBank? question =
                await _unitOfWork.QuestionBankRepository.FirstOrDefaultAsync
                (
                    filter: q => q.QuestionId == id,
                    include: q => q.Include(q => q.Answers)
                );

            if(question == null)
            {
                return NotFound(new { Message = $"No question found with id: {id}"});
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // First transaction
                _unitOfWork.QBAnswersRepository.DeleteRange(question.Answers!);
                _unitOfWork.QuestionBankRepository.Delete(question);

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return Ok(new { Message = "Deleted Successfully"});

            }catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return BadRequest(new { Message = ex.Message});
            }
        }
    }
}
