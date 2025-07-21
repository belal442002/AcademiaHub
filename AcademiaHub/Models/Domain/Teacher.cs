using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademiaHub.Models.Domain
{
    public class Teacher
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(Account))]
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfJoin { get; set; } = DateTime.Now;
        public bool Active_TF { get; set; } = true;

        // Navigation Properties
        public virtual IdentityUser? Account { get; set; }
        public virtual Teacher_Classroom? Teacher_Classroom { get; set; }
        

    }
}
