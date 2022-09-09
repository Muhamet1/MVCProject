using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Areas.Identity.Data;
using Project.Infrastructure;
using Project.Models;

namespace Project.Pages
{
    public class CartModel : PageModel
    {
        private readonly IStoreRepository _context;
        

        public CartModel(IStoreRepository ctx,Cart cartService)
        {
            _context = ctx;
            Cart = cartService;
        }

        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; } = "/";
        public void OnGet(string returnUrl)
        {
            ReturnUrl = returnUrl ?? "/";
            //Cart = HttpContext.Session.GetJson<Cart>("cart") ?? new Cart();
        }

        public IActionResult OnPost(long productId, string returnUrl)
        {
            Product? product = _context.Products
            .FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                Cart.AddItem(product, 1);
            }
            return RedirectToPage(new { returnUrl = returnUrl });
        }

        public IActionResult OnPostRemove (int productId , string returnUrl)
        {
            Cart.RemoveLine(Cart.Lines.First(cl =>
                cl.Product.ProductId == productId).Product);

            return RedirectToPage(new { returnUrl = returnUrl });
        }
    }
}
