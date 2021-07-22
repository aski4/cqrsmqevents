using Domain.Models;
using Infrastructure.Abstraction;

namespace Infrastructure.Repository
{
    public class ArchiveDocumentRepository : Repository<ArchiveDocument>, IArchiveDocumentRepository
    {
        public ArchiveDocumentRepository(ApplicationDbContext context) : base(context)
        {
            
        }

    }
}
