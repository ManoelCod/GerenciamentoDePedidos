using ApiTask.DTOs;
using MediatR;
using System.Text.Json;
using System.Text.Json.Serialization;
using TaskManagement.Application;
using TaskManagement.Application.Service.Interface;

namespace ApiTask.MediatR
{
    public record CriarPedidoCommand(OrderDTO OrderDTO) : IRequest<Guid>;
    public class CriarPedidoHandler : IRequestHandler<CriarPedidoCommand, Guid>
    {
       
        private readonly PedidoApplication _pedidoApplication;
        private readonly JsonSerializerOptions _options;

        public CriarPedidoHandler(PedidoApplication pedidoApplication)
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

        public async Task<Guid> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            if (request.OrderDTO == null)
            {
                throw new ArgumentException("Pedido não pode ser nulo.");
            }

            var pedido = new Pedido(
                Guid.NewGuid(),
                request.OrderDTO.name,
                DateTime.UtcNow
            );

            // Deserializar a string JSON `Carts` em uma lista de `ItemOrderDto`
            var itemsDto = JsonSerializer.Deserialize<List<ItemOrderDto>>(request.OrderDTO.Carts, _options);

            if (itemsDto == null || itemsDto.Count == 0)
            {
                throw new ArgumentException("Items are null or empty");
            }

            foreach (var itemDto in itemsDto)
            {
                var item = new ItemPedido(
                    Guid.NewGuid(), // Gerar novo Guid para o ItemPedido
                    itemDto.Name,
                    itemDto.Quantity,
                    itemDto.Price
                );
                pedido.AdicionarItem(item);
            }

            // Processar o pedido aqui
            await _pedidoApplication.CriarPedidoAsync(pedido);

            return pedido.Id;
        }
    }


}
