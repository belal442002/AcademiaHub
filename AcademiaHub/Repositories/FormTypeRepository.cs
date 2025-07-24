using AcademiaHub.Data;
using AcademiaHub.Interfaces;
using AcademiaHub.Models.Domain;

namespace AcademiaHub.Repositories
{
    public class FormTypeRepository : GenericRepository<FormType>, IFormTypeRepository
    {
        public FormTypeRepository(AcademiaHubDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
