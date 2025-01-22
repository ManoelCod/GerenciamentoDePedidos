using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.IRepositoreis;
using TaskManagement.Domain.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Infrastructure.Repositoreis
{
    public class OrderRepository : IOrderRepository
    {
        private static readonly List<Order> _orders = new List<Order>();

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await Task.FromResult(_orders);
        }

        public async Task<Order> GetByIdAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            return await Task.FromResult(order);
        }

        public async Task AddAsync(Order order)
        {
            _orders.Add(order);
            await Task.CompletedTask;
        }

        public async Task UpdateAsync(Order order)
        {
            var existingOrder = _orders.FirstOrDefault(o => o.Id == order.Id);
            if (existingOrder != null)
            {
                existingOrder.OrderId = order.OrderId;
                existingOrder.Amount = order.Amount;
                existingOrder.Status = order.Status;
                existingOrder.UpdatedAt = order.UpdatedAt;
            }
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Guid id)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null)
            {
                _orders.Remove(order);
            }
            await Task.CompletedTask;
        }
    }
}
