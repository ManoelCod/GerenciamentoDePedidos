using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Pedidos.Commands
{
    public record CriarPedidoCommand(OrderDTO OrderDTO) : IRequest<Guid>;
}
