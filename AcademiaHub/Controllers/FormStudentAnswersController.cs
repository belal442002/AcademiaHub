using AcademiaHub.CustomValidation;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.FormStudentAnswers;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormStudentAnswersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FormStudentAnswersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //[HttpPost]
        //[Route("[action]")]
        //[ValidationModel]
        //public async Task<IActionResult> UploadFormQuestions([FromBody] List<FormStudentAnswerUploadRequest> formStudentAnswerUploadRequests)
        //{
        //    if(!formStudentAnswerUploadRequests.Any())
        //    {
        //        return BadRequest(new { Message = "Empty form"});
        //    }

        //    // To add or update
        //    List<FormStudentAnswers> formStudentAnswers =
        //        _mapper.Map<List<FormStudentAnswers>>(formStudentAnswerUploadRequests);

        //    Student? student = 
        //           await _unitOfWork.StudentRepository.GetByIdAsync(formStudentAnswers[0].StudentId);

        //    // If exist update
        //    List<Models.Domain.FormStudentAnswers> formStudentAnswersDM =
        //        await _unitOfWork.FormStudentAnswerRepository.GetAsync
        //        (
        //            filter: f => f.StudentId == formStudentAnswers[0].StudentId &&
        //                         f.FormId == formStudentAnswers[0].FormId
        //        );

        //    if(formStudentAnswersDM.Any())
        //    {
        //        if(formStudentAnswers.Count != formStudentAnswersDM.Count)
        //        {
        //            return BadRequest(new { Message = "Form confliction"});
        //        }

        //        FormStudentAnswers firstElement = formStudentAnswers[0]; 

        //        for(int i = 0; i < formStudentAnswersDM.Count; i++)
        //        {
        //            if(firstElement.FormId != formStudentAnswers[i].FormId ||
        //               firstElement.StudentId != formStudentAnswers[i].StudentId)
        //            {
        //                return BadRequest(new { Message = "The form has conflict data"});
        //            }

        //            FormStudentAnswers? formStudentAnswer =
        //                formStudentAnswersDM.FirstOrDefault(f => f.QuestionId == formStudentAnswers[i].QuestionId);
        //            if(formStudentAnswer == null)
        //            {
        //                return BadRequest(new { Message = "Wrong values were sent with this form"});
        //            }

        //            formStudentAnswer.StudentAnswer = formStudentAnswers[i].StudentAnswer;
        //        }

        //         _unitOfWork.FormStudentAnswerRepository.UpdateRange(formStudentAnswersDM);
        //        await _unitOfWork.SaveChangesAsync();
        //        return Ok(new { Message = "Form updated successfully"});
        //    }

        //    else
        //    {
        //        await _unitOfWork.FormStudentAnswerRepository.AddRangeAsync(formStudentAnswers);
        //        await _unitOfWork.SaveChangesAsync();
        //        return Ok(new { Message = "Answers addedd successfully"});
        //    }
        //}

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        public async Task<IActionResult> UploadFormQuestions([FromBody] FormAnswersUploadRequest formAnswersUploadRequest)
        {
            Student? student =
                   await _unitOfWork.StudentRepository.GetByIdAsync(formAnswersUploadRequest.StudentId);
            if (student == null)
            {
                return NotFound(new { Message = $"No student found with id: {formAnswersUploadRequest.StudentId}"});
            }

            Classroom? classroom = 
                await _unitOfWork.ClassroomRepository.GetByIdAsync(formAnswersUploadRequest.ClassroomId);
            if (classroom == null)
            {
                return NotFound(new {Message = $"No classroom found with id: {formAnswersUploadRequest.ClassroomId}" });
            }

            QuestionsForm? form =
                await _unitOfWork.QuestionsFormRepository.GetByIdAsync(formAnswersUploadRequest.FormId);
            if (form == null)
            {
                return NotFound(new { Message = $"No Form found with id: {formAnswersUploadRequest.FormId}"});
            }

            List<QuestionBank> classroomQuestions =
                await _unitOfWork.QuestionBankRepository.GetAsync
                (
                    filter: f => f.ClassroomId == formAnswersUploadRequest.ClassroomId
                );

            List<Guid> questionsIds = formAnswersUploadRequest.Answers
                .Select(a => a.QuestionId).ToList();

            List<Guid> existingIds = classroomQuestions.
                Where(q => questionsIds.Contains(q.QuestionId)).Select(q => q.QuestionId).ToList();

            List<Guid> missingIds = questionsIds.Except(existingIds).ToList();
            if(missingIds.Any())
            {
                return BadRequest(new { Message = "Some question IDs are invalid", MissingIds = missingIds });
            }


            // If exist update
            List<Models.Domain.FormStudentAnswers> formStudentAnswers =
                await _unitOfWork.FormStudentAnswerRepository.GetAsync
                (
                    filter: f => f.StudentId == formAnswersUploadRequest.StudentId &&
                                 f.FormId == formAnswersUploadRequest.FormId
                );

            if (!formStudentAnswers.Any())
            {
                List<FormStudentAnswers> studentAnswers = new List<FormStudentAnswers>();
                for (int i = 0; i < formAnswersUploadRequest.Answers.Count; i++)
                {
                    studentAnswers.Add(new FormStudentAnswers
                    {
                        FormId = formAnswersUploadRequest.FormId,
                        StudentId = formAnswersUploadRequest.StudentId,
                        StudentAnswer = formAnswersUploadRequest.Answers[i].StudentAnswer,
                        QuestionId = formAnswersUploadRequest.Answers[i].QuestionId
                    });
                }

                await _unitOfWork.FormStudentAnswerRepository.AddRangeAsync(studentAnswers);
                await _unitOfWork.SaveChangesAsync();
                return Ok(new { Message = "Student answers added successfully"});
            }

            List<Guid> idsOfFormStudentAnswers = formStudentAnswers.Select(f => f.QuestionId).ToList();
            List<Guid> ids = questionsIds.Intersect(idsOfFormStudentAnswers).ToList();
            if(ids.Count != questionsIds.Count || ids.Count != idsOfFormStudentAnswers.Count)
            {
                return BadRequest(new { Message = "Some question IDs are invalid" });
            }

            for(int i = 0; i < formStudentAnswers.Count; i++)
            {
                FormStudentAnswerUploadRequest studentAnswer =
                    formAnswersUploadRequest.Answers.
                    First(a => a.QuestionId == formStudentAnswers[i].QuestionId);
                formStudentAnswers[i].StudentAnswer = studentAnswer.StudentAnswer;
            }

            _unitOfWork.FormStudentAnswerRepository.UpdateRange(formStudentAnswers);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new {Message = "Student answers updated successfully"});
        }

        [HttpGet]
        [Route("[action]/{id:int}")]
        [ValidationModel]
        public async Task<IActionResult> ViewStudentAnswersByFormId(int id)
        {
            Form_Questions? formQuestions =
                await _unitOfWork.FormQuestionsRepository.FirstOrDefaultAsync
                (
                    filter: f => f.FormId == id
                );

            if(formQuestions == null)
            {
                return NotFound(new { Message = $"No form has questions found with id: {id}"});
            }

            List<FormStudentAnswers> studentAnswers =
                await _unitOfWork.FormStudentAnswerRepository.GetAsync
                (
                    filter: a => a.FormId == id,
                    include: answers => answers.Include(a => a.Question).ThenInclude(q => q!.Answers).
                                                Include(a => a.QuestionsForm).ThenInclude(f => f!.FormType).
                                                Include(a => a.QuestionsForm).ThenInclude(f => f!.FormDetails).
                                                Include(a => a.Student)
                );

            if(!studentAnswers.Any())
            {
                return NotFound(new { Message = "No answers found"});
            }


            List<FormAnswerGetRequest> answerGetRequests = 
                studentAnswers.GroupBy(sa => new
                {
                    sa.FormId,
                    sa.StudentId,
                    sa.Student!.Name,
                    sa.QuestionsForm!.FormType!.Type,
                    sa.QuestionsForm.FormDetails!.Title
                }).
                Select(g => new FormAnswerGetRequest
                {
                    StudentId = g.Key.StudentId,
                    StudentName = g.Key.Name,
                    FormId = g.Key.FormId,
                    FormType = g.Key.Type,
                    FormTitle = g.Key.Title,
                    Answers = g.Select(a => new FormStudentAnswerGetRequest
                    {
                       QuestionId = a.QuestionId,
                       QuestionText = a.Question!.QuestionText,
                       StudentAnswer = a.StudentAnswer,
                       CorrectAnswer = a.Question.Answers!.
                                         Where(a => a.Answer_TF == true).
                                         Select(a => a?.Text ?? "True").First()

                    }).ToList()

                }).ToList();

            return Ok(answerGetRequests);
        }
    }
}
