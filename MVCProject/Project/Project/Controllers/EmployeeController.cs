using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Areas.Identity.Data;
using Project.Models;
using Project.Models.ViewModels;

namespace Project.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public int pageSize = 6;

        public EmployeeController(ApplicationDbContext ctx)
        {
            _db = ctx;
        }
        public ViewResult Index(int pageNumber = 1)
            => View(new EmployeeListViewModel
            {
                Employees = _db.Employees
                .OrderBy(e => e.employeeId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = pageSize,
                    TotalItems = _db.Employees.Count()
                }

            });
           

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create (Employee emp)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Add(emp);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _db.Employees == null)
            {
                return NotFound();
            }

            var product = await _db.Employees.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var empFromDb = _db.Employees.Find(id);

            if(empFromDb == null)
            {
                return NotFound();
            }
            return View(empFromDb);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                _db.Employees.Update(emp);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }


        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var empFromDb = _db.Employees.Find(id);
            if(empFromDb == null)
            {
                return NotFound();
            }
            return View(empFromDb);

        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost (int? id)
        {
            var obj = _db.Employees.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Employees.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Employee Deleted Succesfully";
            return RedirectToAction("Index");
        }

    }
}
