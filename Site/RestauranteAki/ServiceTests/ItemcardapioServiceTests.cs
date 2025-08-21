using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;


namespace Service.Tests
{
    [TestClass()]
    public class ItemcardapioServiceTests
    {
        private RestauranteAkiContext context;
        private IItemcardapioService itemcardapioService;

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

            itemcardapioService = new ItemcardapioService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            itemcardapioService.Create(new Itemcardapio() { Id = 4, Nome = "Lasanha", PrecoUnitario = (float)40.0M });
            Assert.AreEqual(4, itemcardapioService.GetAll().Count());
            var item = itemcardapioService.Get(4);
            Assert.AreEqual("Lasanha", item.Nome);
            Assert.AreEqual((float)40.0M, item.PrecoUnitario);
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
            item.PrecoUnitario = 18;
            itemcardapioService.Edit(item);

            item = itemcardapioService.Get(3);
            Assert.IsNotNull(item);
            Assert.AreEqual("Salada Caesar", item.Nome);
            Assert.AreEqual(18, item.PrecoUnitario);
        }

        [TestMethod()]
        public void GetTest()
        {
            var item = itemcardapioService.Get(1);
            Assert.IsNotNull(item);
            Assert.AreEqual("Pizza", item.Nome);
            Assert.AreEqual(35, item.PrecoUnitario);
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
