using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(MaterialType))]
        public int MaterialTypeId { get; set; }
        public string MaterialName { get; set; }
        public string MaterialLink { get; set; }

        // Navigation Properties
        public virtual MaterialType? MaterialType { get; set; }
    }
}
