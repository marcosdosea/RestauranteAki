using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class PedidoItemcardapioServiceTests
    {
        private RestauranteAkiContext context;
        private PedidoItemcardapioService service;

        [TestInitialize]
        public void Setup()
        {
            // Configura banco de dados em memória
            var options = new DbContextOptionsBuilder<RestauranteAkiContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            context = new RestauranteAkiContext(options);
            service = new PedidoItemcardapioService(context);

            // Seed de dados básicos para testes
            SeedDatabase();
        }

        private void SeedDatabase()
        {
            // Adiciona dados necessários para as relações
            var itemCardapio = new Itemcardapio
            {
                Id = 1,
                Nome = "Pizza Margherita",
                PrecoUnitario = 45.50f,
                Porcao = 8,
                DiaSemana = "Segunda",
                Status = true,
                Descricao = "Pizza tradicional",
                Categoria = 1,
                Imagem = new byte[] { 0x01, 0x02 }
            };

            var pessoa = new Pessoa
            {
                Id = 1,
                NomeCompleto = "João Silva",
                Cpf = "12345678900",
                Telefone = "11999999999",
                Bairro = "Centro",
                Cep = "49500000",
                Cidade = "Itabaiana",
                Estado = "SE",
                Logradouro = "Rua das Flores",
                TipoPessoa = "F"
            };

            var mesa = new Mesa
            {
                Id = 1
            };

            var conta = new Contum
            {
                Id = 1,
                Valor = 10,
                Status = "A",
                FormaPagamento = "Dinheiro",
            };

            var personagem = new Personagem
            {
                Id = 1,
                IdentificadorCor = "Azul",  
            };

            var pedido = new Pedido
            {
                Id = 1,
                Status = "S",
                IdConta = 1,
                IdMesa = 1,
                IdPersonagem = 1,
                IdPessoa = 1
            };

            context.Itemcardapios.Add(itemCardapio);
            context.Pessoas.Add(pessoa);
            context.Mesas.Add(mesa);
            context.Conta.Add(conta);
            context.Personagems.Add(personagem);
            context.Pedidos.Add(pedido);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [TestMethod()]
        public void PedidoItemcardapioServiceTest()
        {
            // Arrange & Act
            var service = new PedidoItemcardapioService(context);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            // Act
            var result = service.Create(pedidoItem);

            // Assert
            Assert.AreEqual(1, result);
            var pedidoItemSalvo = context.PedidoItemcardapios
                .FirstOrDefault(p => p.IdPedido == 1 && p.IdItemCardapio == 1);
            Assert.IsNotNull(pedidoItemSalvo);
            Assert.AreEqual(2, pedidoItemSalvo.Quantidade);
        }

        [TestMethod()]
        public void CreateTest_MultiplosItens()
        {
            // Arrange - Primeiro adiciona outro ItemCardapio para ter IDs diferentes
            var itemCardapio2 = new Itemcardapio
            {
                Id = 2,
                Nome = "Pizza Calabresa",
                PrecoUnitario = 40.00f,
                Porcao = 8,
                DiaSemana = "Segunda",
                Status = true,
                Descricao = "Pizza de calabresa",
                Categoria = 1,
                Imagem = new byte[] { 0x01, 0x02 }
            };
            context.Itemcardapios.Add(itemCardapio2);
            context.SaveChanges();

            var pedidoItem1 = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,  // Pizza Margherita
                Quantidade = 2
            };

            var pedidoItem2 = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 2,  // Pizza Calabresa - ID diferente
                Quantidade = 3
            };

            // Act
            service.Create(pedidoItem1);
            service.Create(pedidoItem2);

            // Assert
            var itens = context.PedidoItemcardapios.ToList();
            Assert.AreEqual(2, itens.Count);

            // Validações adicionais
            var item1 = itens.FirstOrDefault(i => i.IdItemCardapio == 1);
            var item2 = itens.FirstOrDefault(i => i.IdItemCardapio == 2);

            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            Assert.AreEqual(2, item1.Quantidade);
            Assert.AreEqual(3, item2.Quantidade);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };
            context.PedidoItemcardapios.Add(pedidoItem);
            context.SaveChanges();

            // Act
            service.Delete(1);

            // Assert
            var pedidoItemDeletado = context.PedidoItemcardapios.FirstOrDefault(x => x.IdItemCardapio == 1);
            Assert.IsNull(pedidoItemDeletado);
        }

        [TestMethod()]
        public void DeleteTest_ItemNaoExistente()
        {
            // Arrange
            int idInexistente = 999;

            // Act
            service.Delete(idInexistente);

            // Assert
            // Não deve lançar exceção
            var count = context.PedidoItemcardapios.Count();
            Assert.AreEqual(0, count);
        }

        [TestMethod()]
        public void EditTest()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };
            context.PedidoItemcardapios.Add(pedidoItem);
            context.SaveChanges();
            context.Entry(pedidoItem).State = EntityState.Detached;

            // Modifica a quantidade
            pedidoItem.Quantidade = 5;

            // Act
            service.Edit(pedidoItem);

            // Assert
            var pedidoItemEditado = context.PedidoItemcardapios
                .AsNoTracking()
                .FirstOrDefault(p => p.IdPedido == 1 && p.IdItemCardapio == 1);
            Assert.IsNotNull(pedidoItemEditado);
            Assert.AreEqual(5, pedidoItemEditado.Quantidade);
        }

        [TestMethod()]
        public void EditTest_AlteracaoCompleta()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };
            context.PedidoItemcardapios.Add(pedidoItem);
            context.SaveChanges();
            context.Entry(pedidoItem).State = EntityState.Detached;

            // Modifica todos os campos possíveis
            pedidoItem.Quantidade = 10;

            // Act
            service.Edit(pedidoItem);

            // Assert
            var pedidoItemEditado = context.PedidoItemcardapios
                .AsNoTracking()
                .FirstOrDefault(p => p.IdPedido == 1 && p.IdItemCardapio == 1);
            Assert.IsNotNull(pedidoItemEditado);
            Assert.AreEqual(10, pedidoItemEditado.Quantidade);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 3
            };
            context.PedidoItemcardapios.Add(pedidoItem);
            context.SaveChanges();

            // Act
            var result = service.Get(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.IdPedido);
            Assert.AreEqual(1, result.IdItemCardapio);
            Assert.AreEqual(3, result.Quantidade);
        }

        [TestMethod()]
        public void GetTest_ItemNaoEncontrado()
        {
            // Act
            var result = service.Get(999);

            // Assert
            Assert.IsNull(result);
        }


        [TestMethod()]
        public void GetAllTest()
        {

            var itemCardapio2 = new Itemcardapio
            {
                Id = 2,
                Nome = "Pizza Calabresa",
                PrecoUnitario = 40.00f,
                Porcao = 8,
                DiaSemana = "Segunda",
                Status = true,
                Descricao = "Pizza de calabresa",
                Categoria = 1,
                Imagem = new byte[] { 0x01, 0x02 }
            };
            context.Itemcardapios.Add(itemCardapio2);
            context.SaveChanges();

            var pedidoItem1 = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,  // Combinação única
                Quantidade = 2
            };

            var pedidoItem2 = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 2,  // Combinação única diferente
                Quantidade = 1
            };

            context.PedidoItemcardapios.AddRange(pedidoItem1, pedidoItem2);
            context.SaveChanges();

            // Act
            var result = service.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            // Validações adicionais
            var resultList = result.ToList();
            Assert.IsTrue(resultList.Any(i => i.IdItemCardapio == 1 && i.Quantidade == 2));
            Assert.IsTrue(resultList.Any(i => i.IdItemCardapio == 2 && i.Quantidade == 1));
        }

        [TestMethod()]
        public void GetAllTest_ListaVazia()
        {
            // Act
            var result = service.GetAll();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod()]
        public void GetAllTest_VerificaAsNoTracking()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };
            context.PedidoItemcardapios.Add(pedidoItem);
            context.SaveChanges();

            // Act
            var result = service.GetAll().ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            // Verifica que não está sendo rastreado
            foreach (var item in result)
            {
                var estado = context.Entry(item).State;
                Assert.AreEqual(EntityState.Detached, estado);
            }
        }

        [TestMethod()]
        public void CreateTest_VerificaRetornoIdCorreto()
        {
            // Arrange
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 4
            };

            // Act
            var idRetornado = service.Create(pedidoItem);

            // Assert
            Assert.AreEqual(1, idRetornado);
            Assert.AreEqual(pedidoItem.IdPedido, idRetornado);
        }
    }
}