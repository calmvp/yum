using Yum.Data;
using Yum.Utility;

namespace Yum.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> CreateAsync(Order order);
        public Task<Order> GetByKeyAsync(string key);
        public Task<IEnumerable<Order>> GetAllAsync(string? userId = null);
        public Task<bool> UpdateOrderStatusAsync(string key, OrderStatusEnum status);
    }
}
