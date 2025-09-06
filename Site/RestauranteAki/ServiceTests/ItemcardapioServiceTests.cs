using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Moq;


namespace Service.Tests
{
    [TestClass()]
    public class ItemcardapioServiceTests
    {
        private RestauranteAkiContext context;
        private IItemcardapioService itemcardapioService;
        private Mock<ICardapioService> mockCardapioService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<RestauranteAkiContext>();
            builder.UseInMemoryDatabase("RestauranteAki");
            var options = builder.Options;

            context = new RestauranteAkiContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var itens = new List<Itemcardapio>
            {
                new() { Id = 1, Nome = "Pizza", PrecoUnitario = (float)35.0m },
                new() { Id = 2, Nome = "Hamburguer", PrecoUnitario = (float)25.0m },
                new() { Id = 3, Nome = "Salada", PrecoUnitario = 15 }
            };

            context.AddRange(itens);
            context.SaveChanges();

            mockCardapioService = new Mock<ICardapioService>();
            mockCardapioService.Setup(s => s.GetByNome(It.IsAny<string>()))
                               .Returns(new List<Cardapio>());

            itemcardapioService = new ItemcardapioService(context, mockCardapioService.Object);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novoItem = new Itemcardapio() { Id = 4, Nome = "Lasanha", PrecoUnitario = 40f };
            var dias = new[] { "Segunda", "Quarta" };

            itemcardapioService.Create(novoItem, dias);

            Assert.AreEqual(4, itemcardapioService.GetAll().Count());
            var item = itemcardapioService.Get(4);
            Assert.IsNotNull(item);
            Assert.AreEqual("Lasanha", item.Nome);
            Assert.AreEqual(40f, item.PrecoUnitario);
            Assert.AreEqual("Segunda,Quarta", item.DiaSemana);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            itemcardapioService.Delete(2);
            Assert.AreEqual(2, itemcardapioService.GetAll().Count());
            var item = itemcardapioService.Get(2);
            Assert.IsNull(item);
        }

        [TestMethod()]
        public void EditTest()
        {
            var item = itemcardapioService.Get(3);
            item.Nome = "Salada Caesar";
            item.PrecoUnitario = 18f;
            itemcardapioService.Edit(item);

            var atualizado = itemcardapioService.Get(3);
            Assert.IsNotNull(atualizado);
            Assert.AreEqual("Salada Caesar", atualizado.Nome);
            Assert.AreEqual(18f, atualizado.PrecoUnitario);
        }

        [TestMethod()]
        public void GetTest()
        {
            var item = itemcardapioService.Get(1);
            Assert.IsNotNull(item);
            Assert.AreEqual("Pizza", item.Nome);
            Assert.AreEqual(35f, item.PrecoUnitario);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var listaItens = itemcardapioService.GetAll();
            Assert.IsInstanceOfType(listaItens, typeof(IEnumerable<Itemcardapio>));
            Assert.IsNotNull(listaItens);
            Assert.AreEqual(3, listaItens.Count());
            Assert.AreEqual(1, listaItens.First().Id);
            Assert.AreEqual("Pizza", listaItens.First().Nome);
        }
    }
}
