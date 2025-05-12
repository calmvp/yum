using System.ComponentModel.DataAnnotations;

namespace Yum.Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int CartLineItemId { get; set; }
        public CartLineItem CartLineItem { get; set; }
    }
}
