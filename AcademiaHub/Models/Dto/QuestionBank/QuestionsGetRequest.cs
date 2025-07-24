using AcademiaHub.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AcademiaHub.Models.Dto.QBAnswers;

namespace AcademiaHub.Models.Dto.QuestionBank
{
    public class QuestionsGetRequest
    {
        public Guid QuestionId { get; set; }
        public int ClassroomId { get; set; }
        public string? ClassroomName { get; set; }
        public int DifficultyId { get; set; }
        public string? DifficultyLevel { get; set; }
        public int QuestionTypeId { get; set; }
        public string? QuestionType { get; set; }
        public string QuestionText { get; set; }

        // Navigation Properties
        public List<QBAnswersGetRequest>? Answers { get; set; }
    }
}
