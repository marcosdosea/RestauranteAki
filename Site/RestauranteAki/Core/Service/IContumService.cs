using static System.Net.Mime.MediaTypeNames;

namespace Core.Service
{
    public interface IContumService
    {
        int Create(Contum conta);
        void Edit(Contum conta);
        void Delete(int id);
        Contum? Get(int id);
        IEnumerable<Contum> GetAll();
        Task<Contum> GetOrCreateContaAtiva(int id);
    }
}