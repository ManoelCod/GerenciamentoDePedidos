using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagement.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application;
using TaskManagement.Application.Service;
using TaskManagement.Application.Service.Interface;
using TaskManagement.Domain.Models;
using TaskManagement.Application.Ultils;
using TaskManagement.Application.Pedidos.Commands;
using TaskManagement.Application.Pedidos.Queries;

namespace ApiTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly PedidoApplication _pedidoApplication;
        private readonly IOrderService _orderService;
        private JsonSerializerOptions options;

        public OrderController(PedidoApplication pedidoApplication, IOrderService orderService, IMediator mediator)
        {
            _mediator = mediator;
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

            var pedidoId = await _mediator.Send(new CriarPedidoCommand(orderDTO));
            return Ok(new { mensagem = "Pedido criado com sucesso!", pedidoId });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderAsync(Guid id, [FromBody] OrderDTO orderDto)
        {
            if (orderDto == null || id.ToString() != orderDto.Id)
            {
                return BadRequest("Pedido não pode ser nulo e o ID deve coincidir.");
            }

            var pedidoExistente = await _mediator.Send(new ObterPedidoPorIdQuery(id));
            if (pedidoExistente == null)
            {
                return NotFound("Pedido não encontrado.");
            }

            await _mediator.Send(new UpdateOrderCommand(id, orderDto));
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