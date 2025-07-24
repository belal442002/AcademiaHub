namespace AcademiaHub.Models.Dto.FormStudentAnswers
{
    public class FormStudentAnswerUploadRequest
    {
        public Guid QuestionId { get; set; }
        public string? StudentAnswer { get; set; }
    }
}
