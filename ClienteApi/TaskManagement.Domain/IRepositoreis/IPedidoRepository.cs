using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.IRepositoreis
{
    public interface IPedidoRepository
    {
        Task<IEnumerable<Pedido>> ObterTodosAsync();
        Task<Pedido> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Pedido pedido);
        Task AtualizarAsync(Pedido pedido);
        Task<bool> RemoverAsync(Guid id);

    }
}
