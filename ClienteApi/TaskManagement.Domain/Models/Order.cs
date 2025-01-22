using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.Models
{
    public class Order
    {
        
        public Guid Id { get; set; }
        public string OrderId { get; set; }
        public string Carts { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public Order(Guid id, string orderId, decimal amount, bool status, DateTime dateTime1, DateTime dateTime2)
        {
            Id = id;
            OrderId = orderId;
            Amount = amount;
            Status = status;
            CreatedAt = dateTime1;
            UpdatedAt = dateTime2;
        }
    }
}
