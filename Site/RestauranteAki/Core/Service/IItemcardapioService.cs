namespace Core.Service;

public interface IItemcardapioService
{
    int Create(Itemcardapio itemcardapio);
    void Edit(Itemcardapio itemcardapio);
    void Delete(int id);
    Itemcardapio? Get(int id);
    IEnumerable<Itemcardapio> GetAll();
    Task<IEnumerable<Itemcardapio>> GetAllAsync();
    IEnumerable<string> GetAllIngredientes();
    IEnumerable<Itemcardapio> GetByCategoria(int categoria);

}
