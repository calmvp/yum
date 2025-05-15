using Yum.Data;
using Yum.Data.Models;
using Yum.Repositories.Interfaces;
using Yum.Services.Extensions;
using Yum.Services.Interfaces;
using Yum.Utility;

namespace Yum.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly ICartLineItemRepository _cartLineItemRepo;
        public OrderService(IOrderRepository orderRepo, ICartLineItemRepository cartLineItemRepo)
        {
            _orderRepo = orderRepo;
            _cartLineItemRepo = cartLineItemRepo;
         }
 
        public async Task<Order> CreateOrderAsync(string userId, CreateOrderRequest request)
        {
            Order order = new Order
            {
                UserId = userId,
                StatusId = (int)OrderStatusEnum.Pending,
                OrderKey = Guid.NewGuid().ToString(),
                OrderDate = DateTime.Now,
                Name = request.OrderName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Items = request.Items.Select(x => x.ToOrderItem()).ToList(),
                OrderTotal = Decimal.ToDouble(request.Items.Sum(i => i.Total))
            };
            var createdOrder = await _orderRepo.CreateAsync(order);
            if (createdOrder is not null)
            {
                await _cartLineItemRepo.MarkItemsOrdered(request.Items.Select(x => x.Id));
                return createdOrder;
            }

            return null;
        }
    }
}
