using ApiTask.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application;

namespace ApiTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly PedidoApplication _pedidoApplication;

        public PedidoController(PedidoApplication pedidoApplication)
        {
            _pedidoApplication = pedidoApplication;
        }


        [HttpPost("create")]
        public IActionResult CreatePedido([FromBody] PedidoDto pedido)
        {
            if (pedido == null)
            {
                return BadRequest("Pedido não pode ser nulo.");
            }
            // Processar o pedido aqui
            //  var pedido = _pedidoApplication.CriarPedido(nomeCliente);

            return Ok(new { mensagem = "Pedido criado com sucesso!" });
        }

        [HttpPost("{pedidoId}/itens")]
        public IActionResult AdicionarItem(Guid pedidoId, [FromBody] ItemDTO itemDto)
        {
            _pedidoApplication.AdicionarItemAoPedido(pedidoId, itemDto.Nome, itemDto.Quantidade, itemDto.ValorUnitario);
            return Ok();
        }

        [HttpGet("{pedidoId}/valorTotal")]
        public IActionResult CalcularValorTotal(Guid pedidoId)
        {
            var valorTotal = _pedidoApplication.CalcularValorTotalPedido(pedidoId);
            return Ok(valorTotal);
        }
    }

}
