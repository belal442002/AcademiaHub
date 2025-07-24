using AcademiaHub.Models.Domain;
using AcademiaHub.Models.Dto.Difficulty;
using AcademiaHub.UnitOfWork;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademiaHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class DifficultiesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DifficultiesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("action")]
        //[Authorize(Roles = "Admin,Teacher")]
        public async Task<IActionResult> GetDifficulties()
        {
            IEnumerable<Difficulty> difficulties = 
                await _unitOfWork.DifficultyRepository.GetAllAsync();

            List<DifficultyGetRequest> difficultyGetRequests = 
                _mapper.Map<List<DifficultyGetRequest>>(difficulties.ToList());

            return Ok(difficultyGetRequests);
        }
    }
}
