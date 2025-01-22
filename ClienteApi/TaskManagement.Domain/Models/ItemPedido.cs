public class ItemPedido
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public int Quantidade { get; private set; }
    public decimal ValorUnitario { get; private set; }

    public ItemPedido(Guid id, string nome, int quantidade, decimal valorUnitario)
    {
        Id = id;
        Nome = nome;
        Quantidade = quantidade;
        ValorUnitario = valorUnitario;
    }

    public decimal ValorTotal => Quantidade * ValorUnitario;
}

public class Pedido
{
    public Guid Id { get; private set; }
    public string NomeCliente { get; private set; }
    public DateTime DataPedido { get; private set; }
    public List<ItemPedido> Itens { get; private set; }

    public Pedido(Guid id, string nomeCliente, DateTime dataPedido)
    {
        Id = id;
        NomeCliente = nomeCliente;
        DataPedido = dataPedido;
        Itens = new List<ItemPedido>();
    }

    public void AdicionarItem(ItemPedido item)
    {
        Itens.Add(item);
    }

    public decimal ValorTotal => Itens.Sum(item => item.ValorTotal);
}
