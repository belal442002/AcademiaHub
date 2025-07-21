using Microsoft.AspNetCore.Identity;

namespace AcademiaHub.Services
{
    public interface ITokenService
    {
        string GenerateJWT(IdentityUser user, List<string> roles);
    }
}
