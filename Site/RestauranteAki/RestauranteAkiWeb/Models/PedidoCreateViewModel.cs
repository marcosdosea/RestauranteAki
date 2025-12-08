namespace RestauranteAkiWeb.Models
{
    public class PedidoCreateViewModel
    {
        public int IdConta { get; set; }
        public int IdMesa { get; set; }

        // Lista de quem pode pedir
        public List<PersonagemSelectionViewModel> Personagens { get; set; } = new();

        // Lista de itens para pedir agrupados por categoria para facilitar a renderização
        public List<CategoriaCardapioViewModel> Categorias { get; set; } = new();
    }

    public class PersonagemSelectionViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string IdentificadorCor { get; set; }
    }

    public class CategoriaCardapioViewModel
    {
        public string NomeCategoria { get; set; }
        public List<ItemCardapioSelectionViewModel> Itens { get; set; } = new();
    }

    public class ItemCardapioSelectionViewModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
    }
}