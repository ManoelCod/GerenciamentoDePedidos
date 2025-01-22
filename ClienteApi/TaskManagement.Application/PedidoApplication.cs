using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Service;

namespace TaskManagement.Application
{
    public class PedidoApplication
    {
        private readonly PedidoService _pedidoService;
        private readonly PedidoService OrderService;
        public PedidoApplication(PedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }

        public Pedido CriarPedido(string nomeCliente)
        {
            return _pedidoService.CriarPedido(nomeCliente, DateTime.Now);
        }

        public void AdicionarItemAoPedido(Guid pedidoId, string nomeItem, int quantidade, decimal valorUnitario)
        {
            var item = new ItemPedido(Guid.NewGuid(), nomeItem, quantidade, valorUnitario);
            _pedidoService.AdicionarItemAoPedido(pedidoId, item);
        }

        public decimal CalcularValorTotalPedido(Guid pedidoId)
        {
            return _pedidoService.CalcularValorTotalPedido(pedidoId);
        }

        public async Task<bool> UpdateOrderAsync(Guid id, Domain.Models.Order order)
        {
            throw new NotImplementedException();
        }

        public async Task GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }
    }

}
