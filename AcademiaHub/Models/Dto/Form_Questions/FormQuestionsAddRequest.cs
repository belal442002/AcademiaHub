using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Form_Questions
{
    public class FormQuestionsAddRequest
    {
        public Guid QuestionId { get; set; }
        public int FormId { get; set; }
    }
}
