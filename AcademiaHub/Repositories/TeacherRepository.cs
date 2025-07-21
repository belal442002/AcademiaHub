using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
