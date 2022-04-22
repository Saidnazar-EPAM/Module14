using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EFCore
{
    public class OrderRepository
    {
        private readonly Module14Context _context;

        public OrderRepository(Module14Context context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetAllAsync(int month = 0, int year = 0, OrderStatus orderStatus = 0, int productId = 0)
        {
            var orders = await _context.Orders.FromSqlRaw("GetOrders @Month = {0}, @Year = {1}, @OrderStatus = {2}, @ProductId = {3}", month, year, orderStatus, productId).ToListAsync();
            return orders;
        }

        public async Task<Order> GetAsync(int orderId)
        {
                var order = await _context.Orders.FirstOrDefaultAsync(b => b.Id == orderId);

                if (order == null)
                    throw new Exception("Order not found");

                return order;
        }

        public async Task<bool> CreateAsync(Order order)
        {
            if (order == null || order.ProductId == 0)
                throw new Exception("Order is invalid");

                if(order.CreatedDate == default)
                    order.CreatedDate = DateTime.Now;

                _context.Orders.Add(order);

                var res = await _context.SaveChangesAsync();

                if (res > 0)
                {
                    return true;
                }
                return false;
        }

        public async Task<bool> UpdateAsync(int orderId, Order order)
        {
            if (order == null || orderId == 0 || order.ProductId == 0 || orderId != order.Id)
                throw new ArgumentNullException("Order is invalid");
            
                if (order.UpdatedDate == default)
                    order.UpdatedDate = DateTime.Now;
                _context.Orders.Update(order);

                var res = await _context.SaveChangesAsync();

                if (res > 0)
                {
                    return true;
                }
                return false;
        }

        public async Task<bool> DeleteAsync(int orderId)
        {
                var order = _context.Orders.Find(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                _context.Orders.Remove(order);

                var res = await _context.SaveChangesAsync();

                if (res > 0)
                {
                    return true;
                }
                return false;
        }

        public async Task BulkDeleteAsync(int month = 0, int year = 0, OrderStatus orderStatus = 0, int productId = 0)
        {
            await _context.Database.ExecuteSqlRawAsync("DeleteOrders @Month = {0}, @Year = {1}, @OrderStatus = {2}, @ProductId = {3}", month, year, orderStatus, productId);
            return;
        }
    }
}
