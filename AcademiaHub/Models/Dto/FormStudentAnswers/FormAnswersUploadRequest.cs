namespace AcademiaHub.Models.Dto.FormStudentAnswers
{
    public class FormAnswersUploadRequest
    {
        public Guid StudentId { get; set; }
        public int FormId { get; set; }
        public int ClassroomId { get; set; }
        public List<FormStudentAnswerUploadRequest> Answers { get; set; }
    }
}
