using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class PessoaService : IPessoaService
    {

        private readonly RestauranteAkiContext context;

        public PessoaService(RestauranteAkiContext context)
        {
            this.context = context;
        }
        public int Create(Pessoa pessoa)
        {
            if (pessoa.DataNascimento.Year > 0 && pessoa.DataNascimento.Year < 150)
            {
                throw new ArgumentException("Data de nascimento inválida.");
            }

            context.Add(pessoa);
            context.SaveChanges();
            return pessoa.Id;
        }

        public void Delete(int id)
        {
            var pessoa = context.Pessoas.Find(id);

            if (pessoa == null)
            {
                throw new ArgumentException("Usuário não encontrado");
            }

            context.Remove(pessoa);
            context.SaveChanges();
        }

        public void Edit(Pessoa pessoa)
        {
            if (pessoa.DataNascimento.Year > 0 && pessoa.DataNascimento.Year < 150)
            {
                throw new ArgumentException("Data de nascimento inválida.");
            }

            context.Update(pessoa);
            context.SaveChanges();
        }

        public Pessoa? Get(int id)
        {
            return context.Pessoas.Find(id);
        }

        public IEnumerable<Pessoa> GetAll()
        {
            return context.Pessoas.AsNoTracking();
        }
    }
}
