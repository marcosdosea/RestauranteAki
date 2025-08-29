namespace Core.Service
{
    public interface ICardapioService
    {
        int Create(Cardapio cardapio);
        void Edit(Cardapio cardapio);
        void Delete(int id);
        Cardapio? Get(int id);
        IEnumerable<Cardapio> GetAll();
        IEnumerable<Cardapio> GetByNome(string nome);
    }
}