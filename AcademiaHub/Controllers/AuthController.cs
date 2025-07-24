using AcademiaHub.CustomValidation;
using AcademiaHub.Helpers;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto;
using AcademiaHub.Services;
using AcademiaHub.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public AuthController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[AllowAnonymous]
        public async Task<IActionResult> RegisterStudent([FromBody] StudentRegisterRequest registerRequest)
        {
            try
            {
                IdentityUser user = new IdentityUser
                {
                    Email = registerRequest.Email,
                    UserName = registerRequest.Email,
                    PhoneNumber = registerRequest.PhoneNumber,
                };

                // Begin Transactions
                await _unitOfWork.BeginTransactionAsync();

                // first transaction
                IdentityResult result = await
                _userManager.CreateAsync(user, registerRequest.Password);

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollBackAsync();
                    return BadRequest(new { Message = "Failed to add user" });
                }

                result = await _userManager.AddToRoleAsync(user, Roles.Student.ToString());

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollBackAsync();
                    return BadRequest(new { Message = "Failed to add role" });
                }

                // second transaction
                Student student = new Student
                {
                    AccountId = user.Id,
                    Name = registerRequest.Name,
                    Address = registerRequest.Address,
                    Gender = registerRequest.Gender,
                    NationalId = registerRequest.NationalId,
                };

                await _unitOfWork.StudentRepository.AddAsync(student);

                // Commit
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return Ok(new { Message = "User created successfully"});
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[AllowAnonymous]
        public async Task<IActionResult> RegisterTeacher([FromBody] TeacherRegisterRequest registerRequest)
        {
            IdentityUser user = new IdentityUser
            {
                Email = registerRequest.Email,
                UserName = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
            };

            try
            {
                // Begin transactions
                await _unitOfWork.BeginTransactionAsync();

                IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (!result.Succeeded)
                {
                    await _unitOfWork.RollBackAsync();
                    return BadRequest(new {Message = "Failed to create user"});
                }

                result = await _userManager.AddToRoleAsync(user, Roles.Teacher.ToString());
                if (!result.Succeeded)
                {
                    await _unitOfWork.RollBackAsync();
                    return BadRequest(new { Message = "Failed to add role" });
                }

                Teacher teacher = new Teacher
                {
                    AccountId = user.Id,
                    Name = registerRequest.Name,
                    Gender = registerRequest.Gender,
                    Address = registerRequest.Address,
                    NationalId = registerRequest.NationalId,
                };

                await _unitOfWork.TeacherRepository.AddAsync(teacher);

                // save changes and commit
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();

                return Ok(new { Message = "User Created successfully" });
            }
            catch(Exception ex)
            {
                await _unitOfWork.RollBackAsync();
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequest registerAdminRequest)
        {
            IdentityUser user = new IdentityUser
            {
                Email = registerAdminRequest.Email,
                UserName = registerAdminRequest.Email,
                PhoneNumber = registerAdminRequest?.PhoneNumber,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerAdminRequest!.Password);

            if(!result.Succeeded)
            {
                return BadRequest(new { Message = "Failed to create admin" });
            }

            result = await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            if(!result.Succeeded)
            {
                return BadRequest(new { Message = "Failed to add role to the admin" });
            }

            return Ok(new { Message = "Admin Created successfully" });

        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            IdentityUser? user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null) return BadRequest(new { Message = "Incorrect Email" });

            bool isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!isCorrectPassword) return BadRequest(new { Message = "Incorrect Password for this email" });

            IList<string>? roles = await _userManager.GetRolesAsync(user);
            if (roles is null || !roles.Any()) return BadRequest(new { Message = "User do not have any role" });

            string token = _tokenService.GenerateJWT(user, roles.ToList());

            LoginResponse response = new LoginResponse
            {
                BearerToken = token,
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPasswordByUserId([FromBody] ResetPasswordByUserIdRequest resetPassword)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(resetPassword.UserId);
            if(user == null)
            {
                return NotFound(new { Message = $"No user found with id: {resetPassword.UserId}"});
            }

            string? token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token == null)
            {
                return BadRequest(new { Message = "Failed to generate the token for reset" });
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
            if(result.Succeeded)
            {
                return Ok(new { Message = $"Password has been reset successfully to {resetPassword.NewPassword}" });
            }
            else
            {
                string errors = string.Join('\n', result.Errors.Select(e => e.Description));
                return BadRequest(new { Message = $"Unable to reset password: {errors}" });
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPasswordByEmail([FromBody] ResetPasswordByEmailRequest resetPassword)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(resetPassword.Email);
            if (user == null)
            {
                return NotFound(new { Message = $"No user found with email: {resetPassword.Email}" });
            }

            string? token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if(token == null)
            {
                return BadRequest(new { Message = "Failed to generate the token for reset" });
            }

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, resetPassword.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new {Message = $"Password has been reset successfully to {resetPassword.NewPassword}" });
            }
            else
            {
                string errors = string.Join('\n', result.Errors.Select(e => e.Description));
                return BadRequest(new { Message = $"Unable to reset password: {errors}" });
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ValidationModel]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            IdentityUser? user =
                await _userManager.FindByEmailAsync(changePasswordRequest.Email);

            if(user == null)
            {
                return NotFound(new {Message = $"No User found with email: {changePasswordRequest.Email}" });
            }

            IdentityResult result =
                await _userManager.
                ChangePasswordAsync(user,
                                    changePasswordRequest.CurrentPassword,
                                    changePasswordRequest.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Password changed successfully return to log in" });
            }
            else
            {
                string errors = string.Join('\n', result.Errors.
                    Select(e => e.Description));
                return BadRequest(new { Message = $"Couldn't update password: {errors}"});
            }
        }

        [HttpDelete]
        [Route("[action]/{userId}")]
        [ValidationModel]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAdminUserByUserId(string userId)
        {
            IdentityUser? user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound(new { Message = $"No user found with id: {userId}"});
            }

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach(string role in roles)
            {
                if(role.Equals(Helpers.Roles.Student.ToString(), StringComparison.OrdinalIgnoreCase) ||
                   role.Equals(Helpers.Roles.Teacher.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return BadRequest(new
                    {
                        Message = "Can't delete an account belongs to student or teacher," +
                        "because of the relations with other tables"
                    });
                }
            }

            IdentityResult result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded)
            {
                string errors = string.Join('\n', result.Errors.Select(e => e.Description));
                return BadRequest(new { Message = $"Failed to delete this user: {errors}" });
            }
            return Ok(new { Message = "Deleted successfully" });
        }
    }
}
