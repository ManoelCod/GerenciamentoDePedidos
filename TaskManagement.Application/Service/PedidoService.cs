using TaskManagement.Domain.IRepositoreis;

namespace TaskManagement.Application.Service
{
    public class PedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public Pedido CriarPedido(string nomeCliente, DateTime dataPedido)
        {
            var pedido = new Pedido(Guid.NewGuid(), nomeCliente, dataPedido);
            _pedidoRepository.Adicionar(pedido);
            return pedido;
        }

        public void AdicionarItemAoPedido(Guid pedidoId, ItemPedido item)
        {
            var pedido = _pedidoRepository.ObterPorId(pedidoId);
            if (pedido != null)
            {
                pedido.AdicionarItem(item);
                _pedidoRepository.Atualizar(pedido);
            }
        }

        public decimal CalcularValorTotalPedido(Guid pedidoId)
        {
            var pedido = _pedidoRepository.ObterPorId(pedidoId);
            return pedido?.ValorTotal ?? 0;
        }
    }

}
