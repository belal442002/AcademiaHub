using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class TeacherClassroomRepository : GenericRepository<Teacher_Classroom>, ITeacherClassroomRepository
    {
        public TeacherClassroomRepository(AcademiaHubDbContext dbContext) : base(dbContext) { }
        {
            
        }
    }
}
