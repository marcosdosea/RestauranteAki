using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

namespace ServiceTests
{
    [TestClass]
    public class PersonagemServiceTests
    {
        private RestauranteAkiContext context;
        private IPersonagemService personagemService;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<RestauranteAkiContext>();
            builder.UseInMemoryDatabase("restaurantedb");
            var options = builder.Options;

            context = new RestauranteAkiContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Criar alguns personagens para teste
            var personagens = new List<Personagem>
            {
                new Personagem { 
                    Id = 1, 
                    IdentificadorCor = "#FF0000", 
                    DataCriacao = DateTime.Now.AddDays(-2),
                    DataAtualizacao = DateTime.Now.AddDays(-2)
                },
                new Personagem { 
                    Id = 2, 
                    IdentificadorCor = "#00FF00", 
                    DataCriacao = DateTime.Now.AddDays(-1),
                    DataAtualizacao = DateTime.Now
                },
                new Personagem {
                    Id = 3,
                    IdentificadorCor = "#FFFF00",
                    DataCriacao = DateTime.Now.AddMinutes(-30),
                    DataAtualizacao = DateTime.Now
                }
            };

            // Adicionar alguns pedidos relacionados
            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, IdMesa = 1, IdPersonagem = 1, Status = "S" },
                new Pedido { Id = 2, IdMesa = 1, IdPersonagem = 2, Status = "P" },
                new Pedido { Id = 3, IdMesa = 2, IdPersonagem = 3, Status = "E" },
                // Pedido extra para o mesmo personagem e mesma mesa
                new Pedido { Id = 4, IdMesa = 1, IdPersonagem = 1, Status = "E" }
            };

            context.AddRange(personagens);
            context.AddRange(pedidos);
            context.SaveChanges();

            personagemService = new PersonagemService(context);
        }

        [TestMethod]
        public async Task AddPersonagemAsync_DeveGerarNovoPersonagemComCorAleatoria()
        {
            // Act
            var novoPersonagem = await personagemService.AddPersonagemAsync(1);

            // Assert
            Assert.IsNotNull(novoPersonagem);
            Assert.IsTrue(novoPersonagem.Id > 0);
            Assert.IsTrue(novoPersonagem.IdentificadorCor.StartsWith("#"));
            Assert.AreEqual(7, novoPersonagem.IdentificadorCor.Length); // Formato #RRGGBB
            Assert.AreEqual(novoPersonagem.DataCriacao.Date, DateTime.Now.Date);
            Assert.AreEqual(novoPersonagem.DataAtualizacao.Date, DateTime.Now.Date);
        }

        [TestMethod]
        public async Task GetPersonagemAsync_DeveRetornarPersonagemExistente()
        {
            // Act
            var personagem = await personagemService.GetPersonagemAsync(1);

            // Assert
            Assert.IsNotNull(personagem);
            Assert.AreEqual("#FF0000", personagem.IdentificadorCor);
        }

        [TestMethod]
        public async Task GetPersonagemAsync_DeveRetornarNullParaIdInexistente()
        {
            // Act
            var personagem = await personagemService.GetPersonagemAsync(999);

            // Assert
            Assert.IsNull(personagem);
        }

        [TestMethod]
        public async Task GetPersonagensByMesaAsync_DeveRetornarPersonagensDaMesa()
        {
            // Act
            var personagens = await personagemService.GetPersonagensByMesaAsync(1);

            // Assert
            Assert.IsNotNull(personagens);
            Assert.AreEqual(2, personagens.Count());
            Assert.IsTrue(personagens.Any(p => p.IdentificadorCor == "#FF0000"));
            Assert.IsTrue(personagens.Any(p => p.IdentificadorCor == "#00FF00"));
        }

        [TestMethod]
        public async Task GetPersonagensByMesaAsync_DeveRetornarListaVaziaParaMesaInexistente()
        {
            // Act
            var personagens = await personagemService.GetPersonagensByMesaAsync(999);

            // Assert
            Assert.IsNotNull(personagens);
            Assert.AreEqual(0, personagens.Count());
        }

        [TestMethod]
        public async Task GetPersonagensByMesaAsync_DeveRetornarPersonagemUnicoMesmoComMultiplosPedidos()
        {
            // Act
            var personagens = await personagemService.GetPersonagensByMesaAsync(1);

            // Assert
            Assert.AreEqual(2, personagens.Count());
            Assert.AreEqual(1, personagens.Count(p => p.Id == 1));
        }

        [TestMethod]
        public async Task DeleteAsync_DeveRemoverPersonagemRecemCriado()
        {
            // Arrange
            var novoPersonagem = await personagemService.AddPersonagemAsync(1);

            // Act
            await personagemService.DeleteAsync(novoPersonagem.Id);

            // Assert
            var personagemDeletado = await personagemService.GetPersonagemAsync(novoPersonagem.Id);
            Assert.IsNull(personagemDeletado);
        }

        [TestMethod]
        public async Task DeleteAsync_NaoDeveRemoverPersonagemJaModificado()
        {
            await personagemService.DeleteAsync(2);

            // Assert
            var personagem = await personagemService.GetPersonagemAsync(2);
            Assert.IsNotNull(personagem);
        }
    }
}
