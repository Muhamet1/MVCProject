using Microsoft.EntityFrameworkCore;
using Project.Areas.Identity.Data;

namespace Project.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public IQueryable<Order> Orders => _context.Orders
                                .Include(o => o.Lines)
                                .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}
