using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace TaskManagement.Application.Pedidos.Queries
{
    public record ObterPedidoPorIdQuery(Guid PedidoId) : IRequest<Pedido>;
}
