using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Domain
{
    public class MaterialType
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; }

        // Navigation Properties
        public virtual List<Material>? Materials { get; set; }
    }
}
