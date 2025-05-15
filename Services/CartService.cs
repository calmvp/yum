using Yum.Data;
using Yum.Repositories.Interfaces;
using Yum.Services.Interfaces;

namespace Yum.Services
{
    public class CartService : ICartService
    {
        private readonly ICartLineItemRepository _cartLineItemRepo;
        private readonly SharedStateService _sharedStateService;
        public CartService(ICartLineItemRepository cartLineItemRepo, SharedStateService sharedStateService)
        {
            _cartLineItemRepo = cartLineItemRepo;
            _sharedStateService = sharedStateService;
        }

        public async Task<bool> UpdateCartAsync(string userId, int productId, int updateByAmount)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            var cartItem = await _cartLineItemRepo.GetCartLineItemAsync(userId, productId);

            var res = false;
            if (cartItem is null)
            {
                cartItem = new CartLineItem()
                {
                    UserId = userId,
                    ProductId = productId,
                    Count = updateByAmount
                };

                res = await _cartLineItemRepo.CreateCartLineItem(cartItem);
            }
            else
            {
                cartItem.Count += updateByAmount;
                if (cartItem.Count <= 0)
                {
                    res = await _cartLineItemRepo.RemoveCartLineItem(cartItem);
                }
                else
                {
                    res = await _cartLineItemRepo.UpdateCartLineItem(cartItem);
                }
            }

            if (res)
            {
                
                _sharedStateService.TotalCartCount = await _cartLineItemRepo.GetActiveCartItemCount(userId);
            }
            return res;
        }
    }
}
