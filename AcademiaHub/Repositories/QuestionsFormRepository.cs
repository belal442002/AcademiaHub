using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class QuestionsFormRepository : GenericRepository<QuestionsForm>, IQuestionsFormRepository
    {
        public QuestionsFormRepository(AcademiaHubDbContext dbContext) : base(dbContext) { }
        
    }
}
