using Yum.Data;
using Yum.Utility;

namespace Yum.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> CreateAsync(Order order);
        public Task<Order> GetByIdAsync(int id);
        public Task<IEnumerable<Order>> GetAllAsync(string? userId = null);
        public Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatusEnum status);
    }
}
