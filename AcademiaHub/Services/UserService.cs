using AcademiaHub.Models.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<List<UserWithRolesDto>> GetAllUsersWithRolesAsync()
        {
            List<IdentityUser> users = await _userManager.Users.ToListAsync();
            List<UserWithRolesDto> result = new List<UserWithRolesDto>();

            foreach (var user in users)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                result.Add(new UserWithRolesDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList()
                });
            }

            return result;
        }
    }
}
