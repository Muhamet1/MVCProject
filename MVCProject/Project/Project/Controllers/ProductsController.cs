using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Areas.Identity.Data;
using Project.Models;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        public int pageSize = 12;

        public ProductsController(ApplicationDbContext context, IPhotoAccessor photoAccessor)
        {
            _context = context;
            _photoAccessor = photoAccessor;
        }

        // GET: Products
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
    

        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            if (ModelState.IsValid)
            {
                var result = await _photoAccessor.AddPhoto(product.file);
                _context.Update(new Product
                {
                    ProductName = product.ProductName,
                    ProductCode = product.ProductCode,
                    ProductDescription = product.ProductDescription,
                    ProductPrice = product.ProductPrice,
                    ProductCategory = product.ProductCategory,
                    PhotoNum = result.PublicId,
                    PhotoUrl = result.Url
                });

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {

            if (ModelState.IsValid)
            {

                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
