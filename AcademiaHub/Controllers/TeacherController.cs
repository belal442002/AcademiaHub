using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Teacher;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class TeacherController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TeacherController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("[action]")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTeachers()
        {
            IEnumerable<Teacher> teachers = await _unitOfWork.TeacherRepository.GetAsync
                (
                  include: t => t.Include(t => t.Account)
                );

            List<TeacherGetRequest> teacherGetRequests = _mapper.Map<List<TeacherGetRequest>>(teachers.ToList());

            return Ok(teacherGetRequests);
        }

        [HttpGet]
        [Route("[action]/{id:guid}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTeacherById([FromRoute] Guid id)
        {
            Teacher? teacher = await _unitOfWork.TeacherRepository.FirstOrDefaultAsync
                (
                  filter: t => t.Id == id,
                  include: teachers => teachers.Include(t => t.Account)
                );

            if(teacher == null)
            {
                return NotFound(new { Message = $"No teacher found with id: {id}" });
            }

            TeacherGetRequest teacherGetRequest = _mapper.Map<TeacherGetRequest>(teacher);

            return Ok(teacherGetRequest);
        }
    }
}
