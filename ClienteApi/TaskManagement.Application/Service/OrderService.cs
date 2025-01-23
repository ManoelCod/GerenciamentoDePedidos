using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Service.Interface;
using TaskManagement.Domain.IRepositoreis;
using TaskManagement.Domain.Models;

namespace TaskManagement.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
         
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        } 
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateOrderAsync(Guid id, Order order)
        {
            var existingOrder = await _orderRepository.GetByIdAsync(id);
            if (existingOrder == null)
            {
                return false;
            }

            existingOrder.OrderId = order.OrderId;
            existingOrder.Amount = order.Amount;
            existingOrder.Status = order.Status;
            existingOrder.UpdatedAt = DateTime.UtcNow;

            await _orderRepository.UpdateAsync(existingOrder);
            return true;
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return false;
            }

            await _orderRepository.DeleteAsync(id);
            return true;
        }
    }
}
