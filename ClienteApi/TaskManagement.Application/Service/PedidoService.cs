using TaskManagement.Domain.IRepositoreis;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;

    public PedidoService(IPedidoRepository pedidoRepository)
    {
        _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
    }

    public async Task<Pedido> CriarPedidoAsync(Pedido pedido)
    {
        // Lógica para criar pedido
        await _pedidoRepository.AdicionarAsync(pedido);
        return pedido;
    }

    public async Task<Pedido> ObterPorIdAsync(Guid id)
    {
        return await _pedidoRepository.ObterPorIdAsync(id);
    }

    public async Task<IEnumerable<Pedido>> ObterTodosAsync()
    {
        return await _pedidoRepository.ObterTodosAsync();
    }

    public async Task<Pedido> AdicionarAsync(Pedido pedido)
    {
        await _pedidoRepository.AdicionarAsync(pedido);
        return pedido;
    }

    public async Task AtualizarAsync(Pedido pedido)
    {
        await _pedidoRepository.AtualizarAsync(pedido);
    }

    public async Task RemoverAsync(Guid id)
    {
        await _pedidoRepository.RemoverAsync(id);
    }

    public async Task<IEnumerable<Pedido>> FiltrarPedidosAsync(DateTime? dataInicio, DateTime? dataFim, string nomeCliente)
    {
        var pedidos = await _pedidoRepository.ObterTodosAsync();

        if (dataInicio.HasValue)
        {
            pedidos = pedidos.Where(pedido => pedido.DataPedido >= dataInicio.Value);
        }

        if (dataFim.HasValue)
        {
            pedidos = pedidos.Where(pedido => pedido.DataPedido <= dataFim.Value);
        }

        if (!string.IsNullOrEmpty(nomeCliente))
        {
            pedidos = pedidos.Where(pedido => pedido.NomeCliente.Contains(nomeCliente, StringComparison.OrdinalIgnoreCase));
        }

        return pedidos;
    }

}
