namespace AcademiaHub.Models.Dto.FormStudentAnswers
{
    public class FormAnswerGetRequest
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public int FormId { get; set; }
        public string FormTitle { get; set; }
        public string FormType { get; set; }
        public List<FormStudentAnswerGetRequest> Answers { get; set; }
    }
}
