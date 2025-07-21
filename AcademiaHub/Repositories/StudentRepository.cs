using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class StudentRepository : GenericRepository<Student>, IStudentRepository
    {
        public StudentRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
