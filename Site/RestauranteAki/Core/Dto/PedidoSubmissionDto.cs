namespace Core.Dto
{
    public class PedidoSubmissionDto
    {
        public int IdConta { get; set; }
        public int IdPersonagem { get; set; }
        public List<ItemPedidoSubmission> Itens { get; set; }
    }
    public class ItemPedidoSubmission
    {
        public int IdItem { get; set; }
        public int Quantidade { get; set; }
    }
}
