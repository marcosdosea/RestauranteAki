using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

namespace ServiceTests
{
    [TestClass]
    public class ContumServiceTests
    {
        private RestauranteAkiContext context;
        private IContumService contaService;

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

            var mesas = new List<Mesa>
            {
                new Mesa() { Id = 1 },
                new Mesa() { Id = 2 }
            };
            context.Mesas.AddRange(mesas);
            context.SaveChanges();

            var contas = new List<Contum>
            {
                new Contum() { Id = 1, Valor = 100, FormaPagamento = "Dinheiro", Status = "A", DataHoraEncerramento = DateTime.Now, IdMesa = 1 },
                new Contum() { Id = 2, Valor = 200, FormaPagamento = "Cartão", Status = "F", DataHoraEncerramento = DateTime.Now, IdMesa = 2 }
            };
            context.AddRange(contas);
            context.SaveChanges();

            contaService = new ContumService(context);
        }

        [TestMethod]
        public void CreateTest()
        {
            var novaConta = new Contum()
            {
                Id = 3,
                Valor = 150,
                FormaPagamento = "Pix",
                Status = "A",
                DataHoraEncerramento = DateTime.Now,
                IdMesa = 1
            };

            contaService.Create(novaConta);

            Assert.AreEqual(3, contaService.GetAll().Count());

            var conta = contaService.Get(3);
            Assert.IsNotNull(conta);
            Assert.AreEqual("Pix", conta.FormaPagamento);
            Assert.AreEqual(1, conta.IdMesa);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var listaContas = contaService.GetAll().OrderBy(c => c.Id).ToList();

            Assert.IsInstanceOfType(listaContas, typeof(IEnumerable<Contum>));
            Assert.IsNotNull(listaContas);
            Assert.AreEqual(2, listaContas.Count());
            Assert.AreEqual(100, listaContas.First().Valor);
            Assert.AreEqual("F", listaContas.Last().Status);
        }

        [TestMethod]
        public void GetTest()
        {
            var conta = contaService.Get(1);

            Assert.IsNotNull(conta);
            Assert.AreEqual(100, conta.Valor);
            Assert.AreEqual("Dinheiro", conta.FormaPagamento);
        }

        [TestMethod]
        public void EditTest()
        {
            var conta = contaService.Get(2);
            conta.Status = "A";
            conta.Valor = 250;
            contaService.Edit(conta);

            var contaEditada = contaService.Get(2);
            Assert.IsNotNull(contaEditada);
            Assert.AreEqual("A", contaEditada.Status);
            Assert.AreEqual(250, contaEditada.Valor);
        }

        [TestMethod]
        public void DeleteTest()
        {
            contaService.Delete(1);

            Assert.AreEqual(1, contaService.GetAll().Count());
            var conta = contaService.Get(1);
            Assert.IsNull(conta);
        }
    }
}
