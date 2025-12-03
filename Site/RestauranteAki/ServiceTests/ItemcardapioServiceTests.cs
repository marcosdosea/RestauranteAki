using Microsoft.VisualStudio.TestTools.UnitTesting;
using Service;
using Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class ItemcardapioServiceTests
    {
        private RestauranteAkiContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RestauranteAkiContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new RestauranteAkiContext(options);
        }

        [TestMethod()]
        public void ItemcardapioServiceTest()
        {
            // Arrange & Act
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);

            // Assert
            Assert.IsNotNull(service);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            var item = new Itemcardapio
            {
                Nome = "Feijoada",
                PrecoUnitario = 35.50f,
                Porcao = 1,
                DiaSemana = "Segunda,Quarta",
                Status = true,
                Descricao = "Feijão preto, carne seca, linguiça",
                Categoria = 1,
                Imagem = new byte[] { 0x01, 0x02 }
            };

            // Act
            int id = service.Create(item);

            // Assert
            Assert.IsTrue(id > 0);
            var itemSalvo = context.Itemcardapios.Find(id);
            Assert.IsNotNull(itemSalvo);
            Assert.AreEqual("Feijoada", itemSalvo.Nome);
            Assert.AreEqual(35.50f, itemSalvo.PrecoUnitario);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            var item = new Itemcardapio
            {
                Nome = "Prato para Deletar",
                PrecoUnitario = 20.00f,
                Porcao = 1,
                DiaSemana = "Terça",
                Status = true,
                Descricao = "Teste",
                Categoria = 2,
                Imagem = new byte[] { 0x01 }
            };
            int id = service.Create(item);

            // Act
            service.Delete(id);

            // Assert
            var itemDeletado = context.Itemcardapios.Find(id);
            Assert.IsNull(itemDeletado);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteTest_ItemNaoExistente_DeveLancarExcecao()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);

            // Act
            service.Delete(999); // ID inexistente
        }

        [TestMethod()]
        public void EditTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            var item = new Itemcardapio
            {
                Nome = "Prato Original",
                PrecoUnitario = 25.00f,
                Porcao = 1,
                DiaSemana = "Quinta",
                Status = true,
                Descricao = "Descrição original",
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            };
            int id = service.Create(item);

            // Act
            var itemParaEditar = context.Itemcardapios.Find(id);
            itemParaEditar.Nome = "Prato Editado";
            itemParaEditar.PrecoUnitario = 30.00f;
            service.Edit(itemParaEditar);

            // Assert
            var itemEditado = context.Itemcardapios.Find(id);
            Assert.AreEqual("Prato Editado", itemEditado.Nome);
            Assert.AreEqual(30.00f, itemEditado.PrecoUnitario);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            var item = new Itemcardapio
            {
                Nome = "Prato Buscar",
                PrecoUnitario = 18.00f,
                Porcao = 2,
                DiaSemana = "Sexta",
                Status = true,
                Descricao = "Ingredientes diversos",
                Categoria = 3,
                Imagem = new byte[] { 0x01 }
            };
            int id = service.Create(item);

            // Act
            var itemEncontrado = service.Get(id);

            // Assert
            Assert.IsNotNull(itemEncontrado);
            Assert.AreEqual("Prato Buscar", itemEncontrado.Nome);
            Assert.AreEqual(id, itemEncontrado.Id);
        }

        [TestMethod()]
        public void GetTest_ItemNaoExistente_DeveRetornarNull()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);

            // Act
            var item = service.Get(999);

            // Assert
            Assert.IsNull(item);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            service.Create(new Itemcardapio
            {
                Nome = "Item 1",
                PrecoUnitario = 10.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });
            service.Create(new Itemcardapio
            {
                Nome = "Item 2",
                PrecoUnitario = 15.00f,
                Porcao = 1,
                DiaSemana = "Terça",
                Status = true,
                Categoria = 2,
                Imagem = new byte[] { 0x01 }
            });

            // Act
            var todos = service.GetAll().ToList();

            // Assert
            Assert.AreEqual(2, todos.Count);
            Assert.IsTrue(todos.Any(i => i.Nome == "Item 1"));
            Assert.IsTrue(todos.Any(i => i.Nome == "Item 2"));
        }

        [TestMethod()]
        public void GetAllIngredientesTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            service.Create(new Itemcardapio
            {
                Nome = "Prato 1",
                PrecoUnitario = 20.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Descricao = "Arroz, Feijão, Carne",
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });
            service.Create(new Itemcardapio
            {
                Nome = "Prato 2",
                PrecoUnitario = 25.00f,
                Porcao = 1,
                DiaSemana = "Terça",
                Status = true,
                Descricao = "Macarrão, Carne, Queijo",
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });

            // Act
            var ingredientes = service.GetAllIngredientes().ToList();

            // Assert
            Assert.IsTrue(ingredientes.Count >= 5);
            Assert.IsTrue(ingredientes.Contains("Arroz", StringComparer.OrdinalIgnoreCase));
            Assert.IsTrue(ingredientes.Contains("Feijão", StringComparer.OrdinalIgnoreCase));
            Assert.IsTrue(ingredientes.Contains("Carne", StringComparer.OrdinalIgnoreCase));
            Assert.IsTrue(ingredientes.Contains("Macarrão", StringComparer.OrdinalIgnoreCase));
            Assert.IsTrue(ingredientes.Contains("Queijo", StringComparer.OrdinalIgnoreCase));
        }

        [TestMethod()]
        public void GetAllIngredientesTest_SemDuplicatas()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            service.Create(new Itemcardapio
            {
                Nome = "Prato 1",
                PrecoUnitario = 20.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Descricao = "Arroz, Feijão",
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });
            service.Create(new Itemcardapio
            {
                Nome = "Prato 2",
                PrecoUnitario = 25.00f,
                Porcao = 1,
                DiaSemana = "Terça",
                Status = true,
                Descricao = "arroz, feijão", // Mesmos ingredientes em minúsculo
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });

            // Act
            var ingredientes = service.GetAllIngredientes().ToList();

            // Assert
            Assert.AreEqual(2, ingredientes.Count);
        }

        [TestMethod()]
        public void GetAllIngredientesTest_ListaVazia()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);

            // Act
            var ingredientes = service.GetAllIngredientes().ToList();

            // Assert
            Assert.AreEqual(0, ingredientes.Count);
        }

        [TestMethod()]
        public void GetByCategoriaTest()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            service.Create(new Itemcardapio
            {
                Nome = "Entrada",
                PrecoUnitario = 12.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });
            service.Create(new Itemcardapio
            {
                Nome = "Prato Principal",
                PrecoUnitario = 30.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Categoria = 2,
                Imagem = new byte[] { 0x01 }
            });
            service.Create(new Itemcardapio
            {
                Nome = "Sobremesa",
                PrecoUnitario = 8.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Categoria = 3,
                Imagem = new byte[] { 0x01 }
            });

            // Act
            var categoria2 = service.GetByCategoria(2).ToList();

            // Assert
            Assert.AreEqual(1, categoria2.Count);
            Assert.AreEqual("Prato Principal", categoria2[0].Nome);
            Assert.AreEqual(2, categoria2[0].Categoria);
        }

        [TestMethod()]
        public void GetByCategoriaTest_CategoriaInexistente()
        {
            // Arrange
            var context = GetInMemoryContext();
            var service = new ItemcardapioService(context);
            service.Create(new Itemcardapio
            {
                Nome = "Item",
                PrecoUnitario = 20.00f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Categoria = 1,
                Imagem = new byte[] { 0x01 }
            });

            // Act
            var resultado = service.GetByCategoria(999).ToList();

            // Assert
            Assert.AreEqual(0, resultado.Count);
        }
    }
}