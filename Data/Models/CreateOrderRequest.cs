using System.ComponentModel.DataAnnotations;

namespace Yum.Data.Models
{
    public class CreateOrderRequest
    {
        [Required]
        [Display(Name = "Order Name")]
        public string OrderName { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public IEnumerable<CartLineItem> Items { get; set; }
    }
}
