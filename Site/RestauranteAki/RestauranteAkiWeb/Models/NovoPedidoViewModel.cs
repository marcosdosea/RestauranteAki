namespace RestauranteAkiWeb.Models
{
    public class NovoPedidoViewModel
    {
        public List<ItemCardapioQuantidadeViewModel> ItensCardapios { get; set; } = [];
        public int IdMesa { get; set; }
        public int IdPersonagem { get; set; }
    }

    public class ItemCardapioQuantidadeViewModel
    {
        public int ItemCardapioId { get; set; }
        public int Quantidade { get; set; }
    }
}
