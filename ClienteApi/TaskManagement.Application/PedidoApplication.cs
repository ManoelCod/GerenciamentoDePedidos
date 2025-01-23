using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagement.Application.Service;

namespace TaskManagement.Application
{
    public class PedidoApplication
    {
        private readonly IPedidoService _pedidoService;

        public PedidoApplication(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }


        public virtual async Task<Pedido> CriarPedidoAsync(Pedido pedido)
        {
            return await _pedidoService.CriarPedidoAsync(pedido);
        }

        public virtual async Task<decimal> CalcularValorTotalPedidoAsync(Guid pedidoId)
        {
            var pedido = await _pedidoService.ObterPorIdAsync(pedidoId);
            return pedido?.ValorTotal ?? 0;
        }

        public virtual async Task<Pedido> ObterPedidoPorIdAsync(Guid id)
        {
            return await _pedidoService.ObterPorIdAsync(id);
        }

        public virtual async Task<IEnumerable<Pedido>> ObterTodosPedidosAsync()
        {
            return await _pedidoService.ObterTodosAsync();
        }

        public virtual async Task AtualizarPedidoAsync(Pedido pedido)
        {
            await _pedidoService.AtualizarAsync(pedido);
        }

        public virtual async Task RemoverPedidoAsync(Guid id)
        {
            await _pedidoService.RemoverAsync(id);
        }

        public virtual async Task<IEnumerable<Pedido>> FiltrarPedidosAsync(DateTime? dataInicio, DateTime? dataFim, string nomeCliente)
        {
            return await _pedidoService.FiltrarPedidosAsync(dataInicio, dataFim, nomeCliente);
        }
    }

}

