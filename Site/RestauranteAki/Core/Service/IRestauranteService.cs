namespace Core.Service
{
    public interface IRestauranteService
    {
        int Create(Restaurante restaurante);
        void Edit(Restaurante restaurante);
        void Delete(int id);
        Restaurante? Get(int id);
        IEnumerable<Restaurante> GetAll();
    }
}