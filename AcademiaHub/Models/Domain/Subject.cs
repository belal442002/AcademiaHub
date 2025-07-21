using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Domain
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public virtual List<Classroom>? Classrooms { get; set; }
    }
}
