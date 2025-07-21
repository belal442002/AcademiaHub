namespace AcademiaHub.Models.Dto
{
    public class UserWithRolesDto
    {
        public string? UserId { get; set; }
        public string ?Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
