using Yum.Data;

namespace Yum.Services.Extensions
{
    public static class CartLineItemExtensions
    {
        public static OrderItem ToOrderItem(this CartLineItem lineItem) 
        {
            return new OrderItem
            {
                Price = Decimal.ToDouble(lineItem.Product.Price),
                ProductName = lineItem.Product.Name,
                CartLineItemId = lineItem.Id
            };
        }
    }
}
