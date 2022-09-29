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
        public int pageSize = 9;

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

        
        public ViewResult Search (string searchString)
           => View(new ProductsListViewModel
           {
               Products = _context.Products
               .Where(p => p.ProductName!.Contains(searchString))
               .OrderBy(p => p.ProductName)
           });

        public ViewResult FilterByPrice(int priceFrom, int priceTo)
          => View(new ProductsListViewModel
          {
              Products = _context.Products
              .Where(p => p.ProductPrice >= priceFrom && p.ProductPrice <= priceTo)
              .OrderBy(p => p.ProductPrice)
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