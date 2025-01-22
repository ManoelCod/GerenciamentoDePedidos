using System.Text.Json;
using ApiTask.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application;
using TaskManagement.Application.Service;
using TaskManagement.Application.Service.Interface;
using TaskManagement.Domain.Models;

namespace ApiTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly PedidoApplication _pedidoApplication;
        private readonly IOrderService _orderService;
        public OrderController(PedidoApplication pedidoApplication)
        {
            _pedidoApplication = pedidoApplication;
        }


        [HttpPost("create")]
        public IActionResult CreatePedido([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest("Pedido não pode ser nulo.");
            }

            var pedido = new Pedido(
                      Guid.NewGuid(),
                     orderDTO.OrderId,
                     DateTime.UtcNow
                      );

            // Deserializar a string JSON `Carts` em uma lista de `ItemPedido`
            var itemsDto = JsonSerializer.Deserialize<List<ItemOrderDto>>(orderDTO.Carts);
            
            if (itemsDto != null)
            {
                foreach (var itemDto in itemsDto)
                {
                  var item = new ItemPedido(
                    pedido.Id,
                    itemDto.Name,
                    itemDto.Quantity,
                    itemDto.Price
                     );
                    pedido.AdicionarItem(item);
                }
            }

            // Processar o pedido aqui
            //  var pedido = _pedidoApplication.CriarPedido(nomeCliente);

            return Ok(new { mensagem = "Pedido criado com sucesso!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] OrderDTO orderDto)
        {
            if (id.ToString() != orderDto.Id)
            {
                return BadRequest();
            }

            var order = new Order(
                id,
                orderDto.OrderId,
                orderDto.Amount,
                orderDto.Status,
                DateTime.Parse(orderDto.CreatedAt),
                DateTime.Parse(orderDto.UpdatedAt)
            );


            var updated = await _pedidoApplication.UpdateOrderAsync(id, order);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var deleted = await _orderService.DeleteOrderAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);

        }

        [HttpGet("{pedidoId}/valorTotal")]
        public IActionResult CalcularValorTotal(Guid pedidoId)
        {
            var valorTotal = _pedidoApplication.CalcularValorTotalPedido(pedidoId);
            return Ok(valorTotal);
        }
    }
}
