using Microsoft.AspNetCore.Mvc;
using Project.Areas.Identity.Data;

namespace Project.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public NavigationMenuViewComponent (ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(_context.Products
                .Select(x=> x.ProductCategory)
                .Distinct()
                .OrderBy(x=> x)
                );
        }
    }
}
