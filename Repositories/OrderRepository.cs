using Microsoft.EntityFrameworkCore;
using Yum.Data;
using Yum.Repositories.Interfaces;
using Yum.Utility;

namespace Yum.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderRepository(ApplicationDbContext db) 
        { 
            _db = db;
        }

        public async Task<Order> CreateAsync(Order order) 
        { 
            order.OrderDate = DateTime.Now;
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(string? userId = null) 
        { 
            if (userId is null)
            {
                return await _db.Orders.Include(u => u.Items).Include(u => u.Status).ToListAsync();
            }
            else
            {
                return await _db.Orders.Include(u => u.Items).Include(u => u.Status).Where(u => u.UserId == userId).ToListAsync();
            }
        }

        public async Task<Order> GetByIdAsync(int orderId)
        {
            return await _db.Orders.Include(u => u.Items).Include(u => u.Status).FirstOrDefaultAsync(u => u.Id == orderId);
        }

        public async Task<Order> UpdateOrderStatusAsync(int orderId, OrderStatusEnum status)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(u => u.Id == orderId);
            if (order is not null)
            {
                order.StatusId = (int)status;
                await _db.SaveChangesAsync();
            }
            return order;
        }
    }
}
