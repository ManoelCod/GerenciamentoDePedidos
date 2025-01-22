using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Service.Interface
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetAllPedidosAsync();
        Task<Pedido> GetPedidoByIdAsync(Guid id);
        Task CreatePedidoAsync(Pedido pedido);
        Task<bool> UpdatePedidoAsync(Guid id, Pedido pedido);
        Task<bool> DeletePedidoAsync(Guid id);
    }
}
