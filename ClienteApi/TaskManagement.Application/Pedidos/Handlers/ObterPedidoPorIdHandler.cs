using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskManagement.Application.Pedidos.Queries;

namespace TaskManagement.Application.Pedidos.Handlers
{
    public class ObterPedidoPorIdHandler : IRequestHandler<ObterPedidoPorIdQuery, Pedido>
    {
        private readonly PedidoApplication _pedidoApplication;

        public ObterPedidoPorIdHandler(PedidoApplication pedidoApplication)
        {
            _pedidoApplication = pedidoApplication;
        }

        public async Task<Pedido> Handle(ObterPedidoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await _pedidoApplication.ObterPedidoPorIdAsync(request.PedidoId);
        }
    }
}
