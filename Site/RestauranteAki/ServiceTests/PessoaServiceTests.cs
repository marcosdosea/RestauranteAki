using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using Microsoft.EntityFrameworkCore;
using Service;
using Xunit;


public class PessoaServiceTests
{
    private RestauranteAkiContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<RestauranteAkiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new RestauranteAkiContext(options);
    }

    [Fact]
    public void CreatePessoa_ShouldAddPessoa()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new PessoaService(context);

        var pessoa = new Pessoa
        {
            NomeCompleto = "Teste",
            Cpf = "12345678900",
            DataNascimento = new DateTime(1990, 1, 1),
            TipoPessoa = "Gestor"
        };

        // Act
        var id = service.Create(pessoa);
        var pessoaFromDb = context.Pessoas.Find(id);

        // Assert
        Assert.IsNotNull(pessoaFromDb);
        Assert.Equals("Teste", pessoaFromDb.NomeCompleto);
        Assert.Equals("Gestor", pessoaFromDb.TipoPessoa);
    }

    [Fact]
    public void DeletePessoa_ShouldRemovePessoa()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new PessoaService(context);

        var pessoa = new Pessoa { NomeCompleto = "Teste", TipoPessoa = "Garcom", DataNascimento = new DateTime(1990, 1, 1) };
        context.Pessoas.Add(pessoa);
        context.SaveChanges();

        // Act
        service.Delete(pessoa.Id);
        var pessoaFromDb = context.Pessoas.Find(pessoa.Id);

        // Assert
        Assert.IsNull(pessoaFromDb);
    }

    [Fact]
    public void EditPessoa_ShouldUpdatePessoa()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new PessoaService(context);

        var pessoa = new Pessoa { NomeCompleto = "Antigo", TipoPessoa = "Gestor", DataNascimento = new DateTime(1990, 1, 1) };
        context.Pessoas.Add(pessoa);
        context.SaveChanges();

        pessoa.NomeCompleto = "Atualizado";

        // Act
        service.Edit(pessoa);
        var pessoaFromDb = context.Pessoas.Find(pessoa.Id);

        // Assert
        Assert.Equals("Atualizado", pessoaFromDb.NomeCompleto);
    }

    [Fact]
    public void GetAllPessoa_ShouldReturnAll()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var service = new PessoaService(context);

        context.Pessoas.AddRange(
            new Pessoa { NomeCompleto = "Pessoa1", TipoPessoa = "Gestor", DataNascimento = new DateTime(1990, 1, 1) },
            new Pessoa { NomeCompleto = "Pessoa2", TipoPessoa = "Garcom", DataNascimento = new DateTime(1992, 1, 1) }
        );
        context.SaveChanges();

        // Act
        var pessoas = service.GetAll().ToList();

        // Assert
        Assert.Equals(2, pessoas.Count);
        // Substitua Assert.Contains(pessoas, p => p.Nome == "Pessoa1"); por:
        Assert.IsTrue(pessoas.Any(p => p.NomeCompleto == "Pessoa1"));
        // Substitua Assert.Contains(pessoas, p => p.Nome == "Pessoa2"); por:
        Assert.IsTrue(pessoas.Any(p => p.NomeCompleto == "Pessoa2"));
    }
}
