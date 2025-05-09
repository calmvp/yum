using System.ComponentModel.DataAnnotations.Schema;

namespace Yum.Data
{
    public class CartLineItem
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
        [NotMapped]
        public decimal Total { get { return Count * this.Product.Price; } }
    }
}
