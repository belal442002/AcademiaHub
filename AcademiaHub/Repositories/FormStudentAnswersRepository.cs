using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class FormStudentAnswersRepository : GenericRepository<FormStudentAnswers>, IFormStudentAnswerRepository
    {
        public FormStudentAnswersRepository(AcademiaHubDbContext dbContext) : base(dbContext) { }
    }
}
