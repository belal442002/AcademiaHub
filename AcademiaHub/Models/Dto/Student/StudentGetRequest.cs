using AcademiaHub.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AcademiaHub.Models.Dto.Student
{
    public class StudentGetRequest
    {
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Name { get; set; }
        public string NationalId { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfJoin { get; set; }
    }
}
