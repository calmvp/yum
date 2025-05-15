using Yum.Data.Models;
using Yum.Data;

namespace Yum.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, CreateOrderRequest request);
    }
}
