using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class FormQuestionsRepository : GenericRepository<Form_Questions>, IFormQuestionsRepository
    {
        public FormQuestionsRepository(AcademiaHubDbContext dbContext) : base(dbContext) { }
        
    }
}
