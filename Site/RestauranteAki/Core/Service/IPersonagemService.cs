namespace Core.Service
{
    public interface IPersonagemService
    {
        int Create(Personagem personagem);
        void Edit(Personagem personagem);
        void Delete(int id);
        Personagem? Get(int id);
        IEnumerable<Personagem> GetAll();
    }
}