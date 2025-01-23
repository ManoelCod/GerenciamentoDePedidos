using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IPedidoService
{
    Task<Pedido> CriarPedidoAsync(Pedido pedido);
    Task<Pedido> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Pedido>> ObterTodosAsync();
    Task<Pedido> AdicionarAsync(Pedido pedido);
    Task AtualizarAsync(Pedido pedido);
    Task RemoverAsync(Guid id);
    Task<IEnumerable<Pedido>> FiltrarPedidosAsync(DateTime? dataInicio, DateTime? dataFim, string nomeCliente);    
}
