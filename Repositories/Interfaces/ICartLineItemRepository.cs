using Yum.Data;

namespace Yum.Repositories.Interfaces
{
    public interface ICartLineItemRepository
    {
        public Task<bool> UpdateCartAsync(string userId, int productId, int updateByAmount);
        public Task<IEnumerable<CartLineItem>> GetAllUserItemsAsync(string userId);
        public Task<bool> ClearUserCartItemsAsync(string userId);

    }
}
