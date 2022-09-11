namespace Project.Models.ViewModels
{
    public class OrderListViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
                = Enumerable.Empty<Order>();

        public PagingInfo PagingInfo { get; set; }
    }
}
