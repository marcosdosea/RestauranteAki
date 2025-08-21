using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

namespace ServiceTests
{
    [TestClass]
    public class PedidoServiceTests
    {
        private RestauranteAkiContext context;
        private IPedidoService pedidoService;

        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            var builder = new DbContextOptionsBuilder<RestauranteAkiContext>();
            builder.UseInMemoryDatabase("restaurantedb");
            var options = builder.Options;

            context = new RestauranteAkiContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var pedidos = new List<Pedido>
            {
                new Pedido() { Id = 1, Status = "S", IdConta = 1, IdMesa = 1, IdPersonagem = 1, IdPessoa = 1},
                new Pedido() { Id = 2, Status = "P", IdConta = 2, IdMesa = 2, IdPersonagem = 2, IdPessoa = 2},
                new Pedido() { Id = 3, Status = "E", IdConta = 2, IdMesa = 2, IdPersonagem = 3, IdPessoa = 2},
            };

            context.AddRange(pedidos);
            context.SaveChanges();

            pedidoService = new PedidoService(context);
        }

        [TestMethod]
        public void CreateTest()
        {
            pedidoService.Create(new Pedido() { Id = 4, Status = "S", IdConta = 3, IdMesa = 1, IdPersonagem = 4, IdPessoa = 1 });

            Assert.AreEqual(4, pedidoService.GetAll().Count());
            var pedido = pedidoService.Get(4);
            Assert.AreEqual("S", pedido.Status);
            Assert.AreEqual(3, pedido.IdConta);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var listaPedidos = pedidoService.GetAll();

            Assert.IsInstanceOfType(listaPedidos, typeof(IEnumerable<Pedido>));
            Assert.IsNotNull(listaPedidos);
            Assert.AreEqual(3, listaPedidos.Count());
            Assert.AreEqual("S", listaPedidos.First().Status);
            Assert.AreEqual("E", listaPedidos.Last().Status);
            Assert.AreEqual(1, listaPedidos.First().IdConta);
        }

        [TestMethod]
        public void GetTest()
        {
            var pedido = pedidoService.Get(1);

            Assert.IsNotNull(pedido);
            Assert.AreEqual(1, pedido.IdMesa);
            Assert.AreEqual(1, pedido.IdPessoa);
        }

        [TestMethod]
        public void EditTest()
        {
            var pedido = pedidoService.Get(3);
            pedido.Status = "P";
            pedidoService.Edit(pedido);

            var pedidoEditado = pedidoService.Get(3);
            Assert.IsNotNull(pedidoEditado);
            Assert.AreEqual("P", pedidoEditado.Status);
            Assert.AreEqual(2, pedidoEditado.IdConta);
        }

        [TestMethod]
        public void DeleteTest()
        {
            pedidoService.Delete(2);

            Assert.AreEqual(2, pedidoService.GetAll().Count());
            var pedido = pedidoService.Get(2);
            Assert.AreEqual(null, pedido);
        }
    }
}
