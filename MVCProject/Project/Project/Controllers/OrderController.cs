using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    [Authorize(Roles = "Basic")]
    public class OrderController : Controller
    {
        private IOrderRepository _context;
        private Cart cart;
        public int pageSize = 12;

        public OrderController(IOrderRepository repo, Cart cartService)
        {
            _context = repo;
            cart = cartService;
        }

        public ViewResult Index(int pageNumber = 1)
           => View(new OrderListViewModel
           {
               Orders = _context.Orders
               .OrderBy(o => o.OrderId)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize),
               PagingInfo = new PagingInfo
               {
                   CurrentPage = pageNumber,
                   ItemsPerPage = pageSize,
                   TotalItems = _context.Orders.Count()
               }

           });

        public ViewResult Checkout() => View (new Order());

        [HttpPost]
        public IActionResult Checkout (Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Nuk mund te boni porosi pasi shporta e juaj eshte e shprazet");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                _context.SaveOrder(order);
                cart.Clear();
                return RedirectToPage("/Completed", new { orderID = order.OrderId , Name = order.Name});

            }
            else
            {
                return View();
            }

        }
    }
}
