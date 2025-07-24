using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.QBAnswers
{
    public class QBAnswersGetRequest
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string? Text { get; set; }
        public bool Answer_TF { get; set; }
    }
}
