using Microsoft.EntityFrameworkCore;
using Yum.Data;
using Yum.Repositories.Interfaces;

namespace Yum.Repositories
{
    public class CartLineItemRepository : ICartLineItemRepository
    {
        private ApplicationDbContext _db;

        public CartLineItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateByAmount)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var cartItem = await _db.CartLineItems.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId);

            if (cartItem is null)
            {
                cartItem = new CartLineItem()
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = updateByAmount
                };

                await _db.CartLineItems.AddAsync(cartItem);
            }
            else
            {
                cartItem.Count += updateByAmount;
                if (cartItem.Count <= 0)
                {
                    _db.Remove(cartItem);
                }
            }
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<CartLineItem>> GetAllUserItemsAsync(string userId)
        {
            return await _db.CartLineItems.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<bool> ClearUserCartItemsAsync(string userId) 
        {
            var cartLineItems = await _db.CartLineItems.Where(u => u.UserId == userId).ToListAsync();
            if (cartLineItems.Count > 0)
            {
                _db.CartLineItems.RemoveRange(cartLineItems);
                return await _db.SaveChangesAsync() > 0;
            }
            return true;
        }

    }
}
