using Yum.Data.Models;
using Yum.Data;

namespace Yum.Services.Interfaces
{
    public interface IOrderService
    {
        Task<string> CreateOrderAsync(string userId, CreateOrderRequest request);
    }
}
