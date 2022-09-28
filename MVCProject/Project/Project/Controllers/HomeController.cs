using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Identity.Data;
using Project.Models;
using System.Diagnostics;
using Project.Models.ViewModels;
using System.Linq;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoreRepository _context;
        public int pageSize = 6;

        public HomeController(ILogger<HomeController> logger, IStoreRepository ctx)
        {
            _logger = logger;
            _context = ctx;
        }

        public ViewResult Index(string? category, int pageNumber = 1)
            => View(new ProductsListViewModel
            {
                Products = _context.Products
                .Where(p => category == null || p.ProductCategory == category)
                .OrderBy(p => p.ProductId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null
                    ? _context.Products.Count()
                    : _context.Products.Where(e =>
                 e.ProductCategory == category).Count()
                },
                CurrentCategory = category

            });

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Random()
        {
            var products =_context.Products;
            var count = _context.Products.Count();
            var rand = new Random();
            var randomProduct = products.Skip(rand.Next(count)).FirstOrDefault();
            if(randomProduct == null)
            {
                return NotFound();
            }

           return View(randomProduct);
        }     


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}