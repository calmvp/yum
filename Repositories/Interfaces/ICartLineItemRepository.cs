using Yum.Data;

namespace Yum.Repositories.Interfaces
{
    public interface ICartLineItemRepository
    {
        public Task<bool> UpdateCartAsync(string userId, int productId, int updateByAmount);
        public Task<bool> CreateCartLineItem(CartLineItem cartLineItem);
        public Task<bool> UpdateCartLineItem(CartLineItem cartLineItem);
        public Task<bool> RemoveCartLineItem(CartLineItem cartLineItem);
        public Task<CartLineItem> GetCartLineItemAsync(string userId, int productId);
        public Task<IEnumerable<CartLineItem>> GetAllUserItemsAsync(string userId);
        public Task<bool> ClearUserCartItemsAsync(string userId);
        public Task<IEnumerable<CartLineItem>> GetCartLineItemsAsync(IEnumerable<int> itemIds);
        public Task<bool> MarkItemsOrdered(IEnumerable<int> itemIds);
        public Task<int> GetActiveCartItemCount(string userId);
    }
}
