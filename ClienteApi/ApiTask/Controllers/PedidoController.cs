using System.Text.Json;
using System.Text.Json.Serialization;
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
        private JsonSerializerOptions options;

        public OrderController(PedidoApplication pedidoApplication, IOrderService orderService)
        {
            _pedidoApplication = pedidoApplication;
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new CustomDecimalConverter()
                }
            };
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePedidoAsync([FromBody] OrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest("Pedido não pode ser nulo.");
            }

            var pedido = new Pedido(
                Guid.NewGuid(),
                orderDTO.name,
                DateTime.UtcNow
            );

            // Deserializar a string JSON `Carts` em uma lista de `ItemOrderDto`
            var itemsDto = JsonSerializer.Deserialize<List<ItemOrderDto>>(orderDTO.Carts, options);

            if (itemsDto == null || itemsDto.Count == 0)
            {
                return BadRequest("Items are null or empty");
            }

            foreach (var itemDto in itemsDto)
            {
                var item = new ItemPedido(
                    Guid.NewGuid(), // Gerar novo Guid para o ItemPedido
                    itemDto.Name,
                    itemDto.Quantity,
                    itemDto.Price
                );
                pedido.AdicionarItem(item);
            }

            // Processar o pedido aqui
            await _pedidoApplication.CriarPedidoAsync(pedido);

            return Ok(new { mensagem = "Pedido criado com sucesso!" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(Guid id, [FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || id.ToString() != orderDto.Id)
            {
                return BadRequest("Pedido não pode ser nulo e o ID deve coincidir.");
            }

            var pedidoExistente = await _pedidoApplication.ObterPedidoPorIdAsync(id);
            if (pedidoExistente == null)
            {
                return NotFound("Pedido não encontrado.");
            }

            pedidoExistente.NomeCliente = orderDto.name;
            pedidoExistente.DataPedido = DateTime.UtcNow;

            // Deserializar a string JSON `Carts` em uma lista de `ItemOrderDto`
            var itemsDto = JsonSerializer.Deserialize<List<ItemOrderDto>>(orderDto.Carts, options);

            if (itemsDto == null || itemsDto.Count == 0)
            {
                return BadRequest("Items are null or empty");
            }

            pedidoExistente.Itens.Clear();
            foreach (var itemDto in itemsDto)
            {
                var item = new ItemPedido(
                    Guid.NewGuid(), // Gerar novo Guid para o ItemPedido
                    itemDto.Name,
                    itemDto.Quantity,
                    itemDto.Price
                );
                pedidoExistente.AdicionarItem(item);
            }

            await _pedidoApplication.AtualizarPedidoAsync(pedidoExistente);

            return Ok(new { mensagem = "Pedido atualizado com sucesso!" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(Guid id)
        {
            var pedidoExistente = await _pedidoApplication.ObterPedidoPorIdAsync(id);
            if (pedidoExistente == null)
            {
                return NotFound("Pedido não encontrado.");
            }

            await _pedidoApplication.RemoverPedidoAsync(id);

            return Ok(new { mensagem = "Pedido removido com sucesso!" });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pedido>> GetOrderByIdAsync(Guid id)
        {
            var orders = await _pedidoApplication.ObterPedidoPorIdAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            return Ok(orders);
        }

        [HttpGet("getAllOrders")]
        public async Task<ActionResult<IEnumerable<Pedido>>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _pedidoApplication.ObterTodosPedidosAsync();
                if (orders == null || !orders.Any())
                {
                    return NotFound("No orders found.");
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{pedidoId}/valorTotal")]
        public async Task<IActionResult> CalcularValorTotalAsync(Guid pedidoId)
        {
            var valorTotal = await _pedidoApplication.CalcularValorTotalPedidoAsync(pedidoId);
            return Ok(valorTotal);
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<IEnumerable<Pedido>>> FiltrarPedidosAsync([FromQuery] DateTime? dataInicio, [FromQuery] DateTime? dataFim, [FromQuery] string nomeCliente)
        {
            if (!dataInicio.HasValue && !dataFim.HasValue && string.IsNullOrEmpty(nomeCliente))
            {
                return BadRequest("Por favor, forneça pelo menos um parâmetro de filtro (dataInicio, dataFim ou nomeCliente).");
            }

            var pedidosFiltrados = await _pedidoApplication.FiltrarPedidosAsync(dataInicio, dataFim, nomeCliente);
            if (pedidosFiltrados == null || !pedidosFiltrados.Any())
            {
                return NotFound("Nenhum pedido encontrado com os critérios fornecidos.");
            }

            return Ok(pedidosFiltrados);
        }


    }
}