namespace Project.Models.ViewModels
{
    public class EmployeeListViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
            = Enumerable.Empty<Employee>();

        public PagingInfo PagingInfo { get; set; }
    }
}
