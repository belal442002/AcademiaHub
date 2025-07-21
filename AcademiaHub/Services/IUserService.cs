using AcademiaHub.Models.Dto;

namespace AcademiaHub.Services
{
    public interface IUserService
    {
        Task<List<UserWithRolesDto>> GetAllUsersWithRolesAsync();
    }
}
