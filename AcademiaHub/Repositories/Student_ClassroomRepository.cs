using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class Student_ClassroomRepository : GenericRepository<Student_Classroom>, IStudent_ClassroomRepository
    {
        public Student_ClassroomRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
