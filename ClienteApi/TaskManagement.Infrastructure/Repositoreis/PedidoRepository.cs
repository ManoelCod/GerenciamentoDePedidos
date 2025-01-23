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

        public Task<Pedido> ObterPorIdAsync(Guid id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(pedido);
        }

        public Task<IEnumerable<Pedido>> ObterTodosAsync()
        {
            return Task.FromResult<IEnumerable<Pedido>>(_pedidos);
        }

        public Task AdicionarAsync(Pedido pedido)
        {
            _pedidos.Add(pedido);
            return Task.CompletedTask;
        }

        public Task AtualizarAsync(Pedido pedido)
        {
            var index = _pedidos.FindIndex(p => p.Id == pedido.Id);
            if (index != -1)
            {
                _pedidos[index] = pedido;
            }
            return Task.CompletedTask;
        }

        public Task<bool> RemoverAsync(Guid id)
        {
            var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
            if (pedido != null)
            {
                _pedidos.Remove(pedido);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

    }

}

