using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class QBAnswersRepository : GenericRepository<QBAnswers>, IQBAnswersRepository
    {
        public QBAnswersRepository(AcademiaHubDbContext dbContext) : base(dbContext) { }
    }
}
