using AcademiaHub.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AcademiaHub.Models.Dto.QBAnswers;

namespace AcademiaHub.Models.Dto.QuestionBank
{
    public class QuestionsAddRequest
    {
        [Required]
        public int ClassroomId { get; set; }
        [Required]
        public int DifficultyId { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
        [Required]
        public string QuestionText { get; set; }

        // Navigation Properties

        [Required]
        public virtual List<QBAnswersAddRequest> Answers { get; set; }
    }
}
