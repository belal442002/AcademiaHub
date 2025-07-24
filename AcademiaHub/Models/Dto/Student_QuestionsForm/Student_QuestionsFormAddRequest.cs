using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.Student_QuestionsForm
{
    public class Student_QuestionsFormAddRequest
    {
        public Guid StudentId { get; set; }
        public int QuestionsFormId { get; set; }
        public double Grade { get; set; }
    }
}
