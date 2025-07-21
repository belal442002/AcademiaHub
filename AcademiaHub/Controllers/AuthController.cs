using AcademiaHub.Helpers;
using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto;
using AcademiaHub.Services;
using AcademiaHub.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
