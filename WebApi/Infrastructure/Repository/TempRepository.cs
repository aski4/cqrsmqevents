using Domain.Models;
using Infrastructure.Abstraction;

namespace Infrastructure.Repository
{
    public class TempRepository : Repository<TempFile>, ITempRepository
    {
        public TempRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
