using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Areas.Identity.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Order> Orders => Set<Order>();

    }
}
