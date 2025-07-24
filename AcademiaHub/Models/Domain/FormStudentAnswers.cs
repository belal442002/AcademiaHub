namespace AcademiaHub.Models.Domain
{
    public class FormStudentAnswers
    {
        public Guid StudentId { get; set; }
        public int FormId { get; set; }
        public Guid QuestionId { get; set; }
        public string? StudentAnswer { get; set; }

        // Navigation Properties

        public Student? Student { get; set; }
        public QuestionsForm? QuestionsForm { get; set; }
        public QuestionBank? Question { get; set; }
    }
}
