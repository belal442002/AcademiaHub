namespace AcademiaHub.Models.Dto.FormStudentAnswers
{
    public class FormStudentAnswerGetRequest
    {
        public Guid QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string CorrectAnswer { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
