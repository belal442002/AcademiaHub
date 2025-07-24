using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Student_QuestionsForm
{
    public class Student_QuestionsFormGetRequest
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public int QuestionsFormId { get; set; }
        public string FormType { get; set; }
        public string FormTitle { get; set; }
        public double Grade { get; set; }
    }
}
