using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class QuestionBankRepository : GenericRepository<QuestionBank>, IQuestionBankRepository
    {
        public QuestionBankRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
