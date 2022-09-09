using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Components
{
    public class CartSummaryViewComponent :ViewComponent
    {
        private Cart cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
