using Microsoft.EntityFrameworkCore;
using System.Collections;
using Yum.Data;
using Yum.Repositories.Interfaces;
using Yum.Services;
using Yum.Services.Interfaces;

namespace Yum.Repositories
{
    public class CartLineItemRepository : ICartLineItemRepository
    {
        private ApplicationDbContext _db;
        private SharedStateService _sharedStateService;

        public CartLineItemRepository(ApplicationDbContext db, SharedStateService sharedStateService)
        {
            _db = db;
            _sharedStateService = sharedStateService;
        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateByAmount)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var cartItem = await _db.CartLineItems.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId && !u.Ordered);

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

            var res = await _db.SaveChangesAsync() > 0;

            if (res)
            {
                _sharedStateService.TotalCartCount = cartItem.Count;
            }
            return res;
        }

        public async Task<CartLineItem> GetCartLineItemAsync(string userId, int productId)
        {
            return await _db.CartLineItems.FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == productId && !u.Ordered);
        }

        public async Task<IEnumerable<CartLineItem>> GetCartLineItemsAsync(IEnumerable<int> itemIds)
        {
            return await _db.CartLineItems.Where(x => itemIds.Contains(x.Id) && !x.Ordered).ToListAsync();
        }

        public async Task<IEnumerable<CartLineItem>> GetAllUserItemsAsync(string userId)
        {
            return await _db.CartLineItems.Include(c => c.Product).Where(u => u.UserId == userId && !u.Ordered).ToListAsync();
        }

        public async Task<bool> CreateCartLineItem(CartLineItem cartLineItem)
        {
            await _db.CartLineItems.AddAsync(cartLineItem);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateCartLineItem(CartLineItem cartLineItem)
        {
            _db.CartLineItems.Update(cartLineItem);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveCartLineItem(CartLineItem cartLineItem)
        {
            _db.Remove(cartLineItem);
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<bool> ClearUserCartItemsAsync(string userId) 
        {
            var cartLineItems = await _db.CartLineItems.Where(u => u.UserId == userId && !u.Ordered).ToListAsync();
            if (cartLineItems.Count > 0)
            {
                _db.CartLineItems.RemoveRange(cartLineItems);
                return await _db.SaveChangesAsync() > 0;
            }
            return true;
        }

        public async Task<bool> MarkItemsOrdered(IEnumerable<int> itemIds)
        {
            IEnumerable<CartLineItem> cartLineItems = await GetCartLineItemsAsync(itemIds);
            foreach (var cartLineItem in cartLineItems) 
            { 
                cartLineItem.Ordered = true;
            }
            return await _db.SaveChangesAsync() > 0;
        }

        public async Task<int> GetActiveCartItemCount(string userId)
        {
            var cartItems = await _db.CartLineItems.Where(u => u.UserId == userId && !u.Ordered).ToListAsync();
            return cartItems.Sum(x => x.Count);
        }
    }
}
