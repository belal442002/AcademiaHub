using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class Student_QuestionsFormRepository : GenericRepository<Student_QuestionsForm>, IStudent_QuestionsFormRepository
    {
        public Student_QuestionsFormRepository(AcademiaHubDbContext dbContext) :base(dbContext)
        {
            
        }
    }
}
