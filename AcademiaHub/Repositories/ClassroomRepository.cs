using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class ClassroomRepository: GenericRepository<Classroom>, IClassroomRepository
    {
        public ClassroomRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
