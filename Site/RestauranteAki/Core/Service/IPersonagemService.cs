namespace Core.Service
{
    public interface IPersonagemService
    {
        Task<Personagem> AddPersonagemAsync(int idConta);
        Task<Personagem?> GetPersonagemAsync(int id);
        Task<IEnumerable<Personagem>> GetPersonagensByMesaAsync(int idMesa);
        Task DeleteAsync(int id);
    }
}