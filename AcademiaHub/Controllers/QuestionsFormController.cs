using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Form_Questions;
using AcademiaHub.Models.Dto.FormDetails;
using AcademiaHub.Models.Dto.QuestionsForm;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsFormController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuestionsFormController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        public async Task<IActionResult> AddQuestionsForm([FromBody] QuestionsFormAddRequest questionsFormAddRequest)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                QuestionsForm questionsForm =
                    _mapper.Map<QuestionsForm>(questionsFormAddRequest);

                FormDetails formDetails =
                    _mapper.Map<FormDetails>(questionsFormAddRequest);

                await _unitOfWork.FormDetailsRepository.AddAsync(formDetails);
                await _unitOfWork.SaveChangesAsync();

                questionsForm.FormDetailsId = formDetails.Id;

                await _unitOfWork.QuestionsFormRepository.AddAsync(questionsForm);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitAsync();

                return Ok(new { Message = "Form and questions added successfully" });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        // Action for teacher
        public async Task<IActionResult> GetFormsByClassroomId([FromRoute] int id)
        {
            Classroom? classroom = 
                await _unitOfWork.ClassroomRepository.GetByIdAsync(id);

            if(classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {id}"});
            }

            List<QuestionsForm> forms =
                await _unitOfWork.QuestionsFormRepository.GetAsync
                (
                    filter: f => f.ClassroomId == id,
                    include: f => f.Include(f => f.FormDetails).Include(f => f.FormType)
                );

            List<QuestionsFormGetRequest> formGetRequests =
                _mapper.Map<List<QuestionsFormGetRequest>>(forms);

            DateTime time = DateTime.Now;

            //formGetRequests = formGetRequests.
            //    Where(f => f.FormDetailsGetRequest.StartDate <= time
            //            && f.FormDetailsGetRequest.EndDate > time).ToList();

            return Ok(formGetRequests);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        // Action for student
        public async Task<IActionResult> GetFormsForStudentByClassroomId([FromRoute] int id)
        {
            Classroom? classroom =
                await _unitOfWork.ClassroomRepository.GetByIdAsync(id);

            if (classroom == null)
            {
                return NotFound(new { Message = $"No classroom found with id: {id}" });
            }

            List<QuestionsForm> forms =
                await _unitOfWork.QuestionsFormRepository.GetAsync
                (
                    filter: f => f.ClassroomId == id,
                    include: f => f.Include(f => f.FormDetails).Include(f => f.FormType)
                );

            DateTime time = DateTime.Now;

            forms = forms.Where(f => f.Active == true &&
                                f.FormDetails!.StartDate <= time &&
                                f.FormDetails.EndDate > time).ToList();

            List<QuestionsFormGetRequest> formGetRequests =
                _mapper.Map<List<QuestionsFormGetRequest>>(forms);

            return Ok(formGetRequests);
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel] 
        // Action for teacher
        public async Task<IActionResult> GetFormById([FromRoute] int id)
        {
            QuestionsForm? form =
                await _unitOfWork.QuestionsFormRepository.FirstOrDefaultAsync
                (
                    filter: f => f.Id == id,
                    include: f => f.Include(f => f.FormDetails).Include(f => f.FormType)
                );

            if(form == null)
            {
                return NotFound(new { Message = $"No form found with id: {id}"});
            }

            QuestionsFormGetRequest formGetRequest =
                _mapper.Map<QuestionsFormGetRequest>(form);

            return Ok(formGetRequest);
        }

        [HttpPut]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        public async Task<IActionResult> DeactivateFormsByClassroomId(int id)
        {
            IEnumerable<QuestionsForm> questionsForms =
                await _unitOfWork.QuestionsFormRepository.GetAsync
                (
                    filter: f => f.Active == true
                );
            int formCount = questionsForms.Count();
            if(formCount == 0)
            {
                return NotFound(new { Message = "No forms found" });
            }
            List<QuestionsForm> questionsFormsList = questionsForms.ToList();
            for(int i = 0; i < formCount; i++)
            {
                questionsFormsList[i].Active = false;
            }

            _unitOfWork.QuestionsFormRepository.UpdateRange(questionsFormsList);

            List<QuestionsFormGetRequest> questionsFormGetRequests =
                _mapper.Map<List<QuestionsFormGetRequest>>(questionsFormsList);

            return Ok(questionsFormGetRequests);
        }

        [HttpDelete]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        public async Task<IActionResult> DeleteFormByFormId(int id)
        {
            await Task.Delay(10);
            return Ok();
        }

        [HttpPut]
        [Route("[action]")]
        [ValidationModel]
        public IActionResult UpdateForm()
        {
            return Ok();
        }
    }
}
