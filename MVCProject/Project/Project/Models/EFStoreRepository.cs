using Project.Areas.Identity.Data;

namespace Project.Models
{
    public class EFStoreRepository : IStoreRepository
    {
        private ApplicationDbContext _context;

        public EFStoreRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IQueryable<Product> Products => _context.Products;
    }
}
