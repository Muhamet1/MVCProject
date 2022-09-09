using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }

        [DisplayName("Product Description")]
        public string ProductDescription { get; set; }

        [DisplayName("Product Price")]
        public int ProductPrice { get; set; }

        [DisplayName("Product Category")]
        public string ProductCategory { get; set; }

        public string? PhotoNum { get; set; }
        public string? PhotoUrl { get; set; }

        [NotMapped]
        public IFormFile file { get; set; }
    }
}
