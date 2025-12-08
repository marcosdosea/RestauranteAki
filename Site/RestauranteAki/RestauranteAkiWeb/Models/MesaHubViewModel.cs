namespace RestauranteAkiWeb.Models
{
    public class MesaHubViewModel
    {
        public int IdConta { get; set; }
        public int IdMesa { get; set; }
        public string NomeMesa => $"Mesa {IdMesa}";

        // Card Financeiro
        public decimal TotalAtual { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Servico { get; set; }

        // Listas para o Accordion
        public List<HubGrupoPedidoViewModel> GruposPedidos { get; set; } = new List<HubGrupoPedidoViewModel>();
    }

    public class HubGrupoPedidoViewModel
    {
        public string Titulo { get; set; } // "Itens da mesa", "Cliente 1"
        public int? IdPersonagem { get; set; }
        public List<HubItemExtratoViewModel> Itens { get; set; } = new List<HubItemExtratoViewModel>();

        // ID único para o HTML do accordion funcionar corretamente
        public string AccordionId => IdPersonagem.HasValue ? $"p-{IdPersonagem}" : "mesa-geral";
    }

    public class HubItemExtratoViewModel
    {
        public int IdItemPedido { get; set; } // Para editar/remover
        public string NomeItem { get; set; }
        public int Quantidade { get; set; }

        // Dados de Status para visualização (Ex: "Entregue", "Em preparo")
        public string StatusTexto { get; set; }
        public string StatusCorCss { get; set; } // Classe CSS: "text-success", "text-warning", etc.
    }
}