using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.FormDetails;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.QuestionsForm
{
    public class QuestionsFormAddRequest
    {
        public int ClassroomId { get; set; }
        public int FormTypeId { get; set; }

        // Navigation Properties
        public FormDetailsAddRequest FormDetailsAddRequest { get; set; }
    }
}
