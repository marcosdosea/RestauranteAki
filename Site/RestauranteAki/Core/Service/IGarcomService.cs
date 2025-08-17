namespace Core.Service
{
    public interface IGarcomService
    {
        int Create(Garcom garcom);
        void Edit(Garcom garcom);
        void Delete(int id);
        Garcom? Get(int id);
        IEnumerable<Garcom> GetAll();
    }
}