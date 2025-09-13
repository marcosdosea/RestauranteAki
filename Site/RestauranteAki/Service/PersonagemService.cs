using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PersonagemService : IPersonagemService
    {
        public readonly RestauranteAkiContext context;

        public PersonagemService(RestauranteAkiContext context)
        {
            this.context = context;
        }

        public async Task<Personagem> AddPersonagemAsync()
        {
            var horaAtual = DateTime.Now;

            var novoPersonagem = new Personagem
            {
                IdentificadorCor = $"#{new Random().Next(0x1000000):X6}", // Gera uma cor aleatória
                DataCriacao = horaAtual,
                DataAtualizacao = horaAtual
            };

            context.Personagems.Add(novoPersonagem);
            await context.SaveChangesAsync();

            return novoPersonagem;
        }

        public async Task DeleteAsync(int id)
        {
            var personagem = await context.Personagems.FindAsync(id);
            if (personagem != null && personagem.DataCriacao == personagem.DataAtualizacao)
            {
                context.Pedidos.RemoveRange(context.Pedidos.Where(p => p.IdPersonagem == id));
                context.Personagems.Remove(personagem);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Personagem?> GetPersonagemAsync(int id)
        {
            return await context.Personagems.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Personagem>> GetPersonagensByMesaAsync(int idMesa)
        {
            //var personagens = await context.Pedidos
            //    .Where(p => p.IdMesa == idMesa)
            //    .Select(p => p.IdPersonagemNavigation)
            //    .Distinct()
            //    .ToListAsync();

            var personagens = await context.Personagems
                .Where(p => p.Pedidos.Any(pe => pe.IdMesa == idMesa))
                .ToListAsync();

            return personagens;
        }
    }
}
