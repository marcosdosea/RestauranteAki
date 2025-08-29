namespace Core.Service
{
    public interface IItemcardapioService
    {
        int Create(Itemcardapio itemcardapio, string[] diasSemana, ICardapioService cardapioService);
        void Edit(Itemcardapio itemcardapio);
        void Delete(int id);
        Itemcardapio? Get(int id);
        IEnumerable<Itemcardapio> GetAll();
    }
}