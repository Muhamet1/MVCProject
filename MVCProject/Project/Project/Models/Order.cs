using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; } = new List<CartLine>();

        [Required(ErrorMessage = "Ju lutem shkruani emrin tuaj !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ju lutem shkruani mbiemrin tuaj !")]
        public string SurName { get; set; }

        [MinLength(9)]
        public string TelNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

    }
}
