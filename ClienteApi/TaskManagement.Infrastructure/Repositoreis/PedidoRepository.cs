using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.IRepositoreis;

namespace TaskManagement.Infrastructure.Repositoreis
{
    public class PedidoRepository : IPedidoRepository
    {
        private static readonly List<Pedido> _pedidos = new List<Pedido>();

        public Pedido ObterPorId(Guid id)
        {
            return _pedidos.FirstOrDefault(p => p.Id == id);
        }

        public void Adicionar(Pedido pedido)
        {
            _pedidos.Add(pedido);
        }

        public void Atualizar(Pedido pedido)
        {
            var index = _pedidos.FindIndex(p => p.Id == pedido.Id);
            if (index >= 0)
            {
                _pedidos[index] = pedido;
            }
        }

        public void Remover(Guid id)
        {
            var pedido = ObterPorId(id);
            if (pedido != null)
            {
                _pedidos.Remove(pedido);
            }
        }
    }

}
