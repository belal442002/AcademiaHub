using AcademiaHub.Models.Dto.QuestionBank;

namespace AcademiaHub.Models.Dto.Form_Questions
{
    public class FormQuestionsGetRequest
    {
        public Guid QuestionId { get; set; }
        public int FormId { get; set; }
        public int QuestionTypeId { get; set; }
        public string QuestionType { get; set; }
        public string QuestionText { get; set; }
        public List<string> Choices { get; set; } = new List<string>();
        public string? StudentAnswer { get; set; }
    }
}
