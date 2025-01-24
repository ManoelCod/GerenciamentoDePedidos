using System.Text.Json;
using System.Text.Json.Serialization;
using ApiTask.DTOs;
using MediatR;
using TaskManagement.Application;

namespace ApiTask.MediatR
{
    public record UpdateOrderCommand(Guid Id, OrderDTO OrderDTO) : IRequest<Unit>;
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly PedidoApplication _pedidoApplication;
        private readonly JsonSerializerOptions _options;

        public UpdateOrderHandler(PedidoApplication pedidoApplication)
        {
            _pedidoApplication = pedidoApplication;
            _options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
            {
                new JsonStringEnumConverter(),
                new CustomDecimalConverter()
            }
            };
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderDTO == null || request.Id.ToString() != request.OrderDTO.Id)
            {
                throw new ArgumentException("Pedido não pode ser nulo e o ID deve coincidir.");
            }

            var pedidoExistente = await _pedidoApplication.ObterPedidoPorIdAsync(request.Id);
            if (pedidoExistente == null)
            {
                throw new KeyNotFoundException("Pedido não encontrado.");
            }

            pedidoExistente.NomeCliente = request.OrderDTO.name;
            pedidoExistente.DataPedido = DateTime.UtcNow;

            var itemsDto = JsonSerializer.Deserialize<List<ItemOrderDto>>(request.OrderDTO.Carts, _options);

            if (itemsDto == null || itemsDto.Count == 0)
            {
                throw new ArgumentException("Items are null or empty");
            }

            pedidoExistente.Itens.Clear();
            foreach (var itemDto in itemsDto)
            {
                var item = new ItemPedido(
                    Guid.NewGuid(), // Gerar novo Guid para o ItemPedido
                    itemDto.Name,
                    itemDto.Quantity,
                    itemDto.Price
                );
                pedidoExistente.AdicionarItem(item);
            }

            await _pedidoApplication.AtualizarPedidoAsync(pedidoExistente);

            return Unit.Value;
        }
    }

}
