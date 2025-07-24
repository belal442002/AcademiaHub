using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class FormDetailsRepository : GenericRepository<FormDetails>, IFormDetailsRepository
    {
        public FormDetailsRepository(AcademiaHubDbContext dbContext) : base(dbContext) { }
        
    }
}
