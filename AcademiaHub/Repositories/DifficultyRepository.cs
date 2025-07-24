using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class DifficultyRepository : GenericRepository<Difficulty>, IDifficultyRepository
    {
        public DifficultyRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
