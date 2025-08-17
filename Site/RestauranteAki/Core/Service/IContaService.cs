namespace Core.Service
{
    public interface IContaService
    {
        int Create(Contum conta);
        void Edit(Contum conta);
        void Delete(int id);
        Contum? Get(int id);
        IEnumerable<Contum> GetAll();
    }
}