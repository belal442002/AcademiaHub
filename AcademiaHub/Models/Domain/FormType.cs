using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Domain
{
    public class FormType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        // Navigation Properties
        public virtual List<QuestionsForm>? Forms { get; set; }
    }
}
