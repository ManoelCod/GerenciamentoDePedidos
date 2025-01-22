using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Domain.IRepositoreis
{
    public interface IPedidoRepository
    {
        Pedido ObterPorId(Guid id);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);
        void Remover(Guid id);
    }

}
