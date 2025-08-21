using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using System;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class CardapioServiceTests
    {
        private RestauranteAkiContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<RestauranteAkiContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new RestauranteAkiContext(options);
        }

        [TestMethod()]
        public void CreateCardapio_ShouldAddCardapio()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CardapioService(context);

            var cardapio = new Cardapio
            {
                Nome = "Cardápio Teste",
                DataInicio = new DateTime(2025, 1, 1),
                DataFim = new DateTime(2025, 12, 31),
                Ativo = 1,
                IdRestaurante = 1
            };

            // Act
            var id = service.Create(cardapio);
            var cardapioFromDb = context.Cardapios.Find(id);

            // Assert
            Assert.IsNotNull(cardapioFromDb);
            Assert.AreEqual("Cardápio Teste", cardapioFromDb.Nome);
            Assert.AreEqual(1, cardapioFromDb.Ativo);
            Assert.AreEqual(1, cardapioFromDb.IdRestaurante);
        }

        [TestMethod()]
        public void DeleteCardapio_ShouldRemoveCardapio()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CardapioService(context);

            var cardapio = new Cardapio
            {
                Nome = "Cardápio X",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddMonths(1),
                Ativo = 1,
                IdRestaurante = 1
            };
            context.Cardapios.Add(cardapio);
            context.SaveChanges();

            // Act
            service.Delete(cardapio.Id);
            var cardapioFromDb = context.Cardapios.Find(cardapio.Id);

            // Assert
            Assert.IsNull(cardapioFromDb);
        }

        [TestMethod()]
        public void EditCardapio_ShouldUpdateCardapio()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CardapioService(context);

            var cardapio = new Cardapio
            {
                Nome = "Antigo",
                DataInicio = DateTime.Today,
                DataFim = DateTime.Today.AddMonths(1),
                Ativo = 1,
                IdRestaurante = 1
            };
            context.Cardapios.Add(cardapio);
            context.SaveChanges();

            // Atualizando
            cardapio.Nome = "Atualizado";
            cardapio.Ativo = 0;

            // Act
            service.Edit(cardapio);
            var cardapioFromDb = context.Cardapios.Find(cardapio.Id);

            // Assert
            Assert.AreEqual("Atualizado", cardapioFromDb.Nome);
            Assert.AreEqual(0, cardapioFromDb.Ativo);
        }

        [TestMethod()]
        public void GetAllCardapio_ShouldReturnAll()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CardapioService(context);

            context.Cardapios.AddRange(
                new Cardapio
                {
                    Nome = "Cardápio 1",
                    DataInicio = DateTime.Today,
                    DataFim = DateTime.Today.AddMonths(1),
                    Ativo = 1,
                    IdRestaurante = 1
                },
                new Cardapio
                {
                    Nome = "Cardápio 2",
                    DataInicio = DateTime.Today,
                    DataFim = DateTime.Today.AddMonths(2),
                    Ativo = 0,
                    IdRestaurante = 2
                }
            );
            context.SaveChanges();

            // Act
            var cardapios = service.GetAll().ToList();

            // Assert
            Assert.AreEqual(2, cardapios.Count);
            Assert.IsTrue(cardapios.Any(c => c.Nome == "Cardápio 1" && c.Ativo == 1));
            Assert.IsTrue(cardapios.Any(c => c.Nome == "Cardápio 2" && c.Ativo == 0));
        }
    }
}
