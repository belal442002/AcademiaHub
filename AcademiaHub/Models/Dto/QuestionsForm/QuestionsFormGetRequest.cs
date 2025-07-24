using AcademiaHub.Models.Dto.FormDetails;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Dto.QuestionsForm
{
    public class QuestionsFormGetRequest
    {
        public int Id { get; set; }
        public int ClassroomId { get; set; }
        public string ClassroomName { get; set; }
        public int FormDetailsId { get; set; }
        public int FormTypeId { get; set; }
        public string FormType { get; set; }
        public bool Active { get; set; }
        public double Duration { get; set; }
        public FormDetailsGetRequest FormDetailsGetRequest { get; set; }
    }
}
