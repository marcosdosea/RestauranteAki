using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestauranteAkiWeb.Controllers;
using RestauranteAkiWeb.Models;
using Core;
using Core.Service;
using AutoMapper;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestauranteAkiWeb.Controllers.Tests
{
    [TestClass()]
    public class ItemcardapioControllerTests
    {
        private Mock<IItemcardapioService> mockService;
        private Mock<IMapper> mockMapper;
        private ItemcardapioController controller;

        [TestInitialize]
        public void Setup()
        {
            mockService = new Mock<IItemcardapioService>();
            mockMapper = new Mock<IMapper>();
            controller = new ItemcardapioController(mockMapper.Object, mockService.Object);
        }

        [TestMethod()]
        public void ItemcardapioControllerTest()
        {
            // Arrange & Act
            var ctrl = new ItemcardapioController(mockMapper.Object, mockService.Object);

            // Assert
            Assert.IsNotNull(ctrl);
        }

        [TestMethod()]
        public void IndexTest()
        {
            // Arrange
            var itemcardapios = new List<Itemcardapio>
            {
                new Itemcardapio { Id = 1, Nome = "Item 1" },
                new Itemcardapio { Id = 2, Nome = "Item 2" }
            };
            var viewModels = new List<ItemcardapioViewModel>
            {
                new ItemcardapioViewModel { Id = 1, Nome = "Item 1" },
                new ItemcardapioViewModel { Id = 2, Nome = "Item 2" }
            };

            mockService.Setup(s => s.GetAll()).Returns(itemcardapios);
            mockMapper.Setup(m => m.Map<List<ItemcardapioViewModel>>(itemcardapios)).Returns(viewModels);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as List<ItemcardapioViewModel>;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            // Arrange
            var itemcardapio = new Itemcardapio { Id = 1, Nome = "Feijoada" };
            var viewModel = new ItemcardapioViewModel { Id = 1, Nome = "Feijoada" };

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map<ItemcardapioViewModel>(itemcardapio)).Returns(viewModel);

            // Act
            var result = controller.Details(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ItemcardapioViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Feijoada", model.Nome);
        }

        [TestMethod()]
        public void CreateGetTest()
        {
            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ItemcardapioViewModel;
            Assert.IsNotNull(model);
        }

        [TestMethod()]
        public void CreatePostTest_ModeloValido()
        {
            // Arrange
            var viewModel = new ItemcardapioViewModel
            {
                Nome = "Feijoada",
                Descricao = "Feijão preto, carne",
                PrecoUnitario = 35.50f,
                Porcao = 2,
                Status = true,
                Categoria = "Prato Principal"
            };
            var itemcardapio = new Itemcardapio
            {
                Nome = "Feijoada",
                Descricao = "Feijão preto, carne",
                PrecoUnitario = 35.50f,
                Porcao = 2,
                Status = true
            };

            mockMapper.Setup(m => m.Map<Itemcardapio>(viewModel)).Returns(itemcardapio);
            mockService.Setup(s => s.Create(It.IsAny<Itemcardapio>())).Returns(1);

            // Act
            var result = controller.Create(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Create(It.IsAny<Itemcardapio>()), Times.Once);
        }

        [TestMethod()]
        public void CreatePostTest_DescricaoVazia_DeveRetornarErro()
        {
            // Arrange
            var viewModel = new ItemcardapioViewModel
            {
                Nome = "Feijoada",
                Descricao = "",
                PrecoUnitario = 35.50f,
                Porcao = 2,
                Categoria = "Prato Principal"
            };

            // Act
            var result = controller.Create(viewModel) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsTrue(controller.ModelState.ContainsKey(nameof(viewModel.Descricao)));
        }

        [TestMethod()]
        public void CreatePostTest_ComImagemUpload()
        {
            // Arrange
            var viewModel = new ItemcardapioViewModel
            {
                Nome = "Feijoada",
                Descricao = "Feijão preto, carne",
                PrecoUnitario = 35.50f,
                Porcao = 2,
                Status = true,
                Categoria = "Prato Principal"
            };

            var fileContent = "imagem-fake";
            var fileName = "teste.jpg";
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
            var formFile = new FormFile(stream, 0, stream.Length, "ImagemUpload", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };
            viewModel.ImagemUpload = formFile;

            var itemcardapio = new Itemcardapio
            {
                Nome = "Feijoada",
                Descricao = "Feijão preto, carne"
            };

            mockMapper.Setup(m => m.Map<Itemcardapio>(viewModel)).Returns(itemcardapio);
            mockService.Setup(s => s.Create(It.IsAny<Itemcardapio>())).Returns(1);

            // Act
            var result = controller.Create(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Create(It.Is<Itemcardapio>(i => i.Imagem != null && i.Imagem.Length > 0)), Times.Once);
        }

        [TestMethod()]
        public void EditGetTest()
        {
            // Arrange
            var itemcardapio = new Itemcardapio
            {
                Id = 1,
                Nome = "Feijoada",
                Imagem = Encoding.UTF8.GetBytes("imagem-teste")
            };
            var viewModel = new ItemcardapioViewModel { Id = 1, Nome = "Feijoada" };

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map<ItemcardapioViewModel>(itemcardapio)).Returns(viewModel);

            // Act
            var result = controller.Edit(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ItemcardapioViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
            Assert.IsNotNull(model.ImagemAtual);
        }

        [TestMethod()]
        public void EditGetTest_ItemNaoEncontrado()
        {
            // Arrange
            mockService.Setup(s => s.Get(999)).Returns((Itemcardapio)null);

            // Act
            var result = controller.Edit(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void EditPostTest_ModeloValido()
        {
            // Arrange
            var viewModel = new ItemcardapioViewModel
            {
                Id = 1,
                Nome = "Feijoada Editada",
                Descricao = "Feijão preto, carne editada",
                PrecoUnitario = 40.00f,
                Porcao = 3,
                Categoria = "Prato Principal"
            };
            var itemcardapio = new Itemcardapio
            {
                Id = 1,
                Nome = "Feijoada",
                Imagem = new byte[] { 0x01 }
            };

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map(viewModel, itemcardapio));

            // Act
            var result = controller.Edit(1, viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Edit(itemcardapio), Times.Once);
        }

        [TestMethod()]
        public void EditPostTest_IdsDiferentes()
        {
            // Arrange
            var viewModel = new ItemcardapioViewModel { Id = 1 };

            // Act
            var result = controller.Edit(2, viewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void EditPostTest_ItemNaoEncontrado()
        {
            // Arrange
            var viewModel = new ItemcardapioViewModel
            {
                Id = 999,
                Nome = "Item",
                Descricao = "Descrição"
            };
            mockService.Setup(s => s.Get(999)).Returns((Itemcardapio)null);

            // Act
            var result = controller.Edit(999, viewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void DeleteGetTest()
        {
            // Arrange
            var itemcardapio = new Itemcardapio
            {
                Id = 1,
                Nome = "Feijoada",
                Imagem = Encoding.UTF8.GetBytes("imagem")
            };
            var viewModel = new ItemcardapioViewModel { Id = 1, Nome = "Feijoada" };

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map<ItemcardapioViewModel>(itemcardapio)).Returns(viewModel);

            // Act
            var result = controller.Delete(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            var model = result.Model as ItemcardapioViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod()]
        public void DeleteGetTest_ItemNaoEncontrado()
        {
            // Arrange
            mockService.Setup(s => s.Get(999)).Returns((Itemcardapio)null);

            // Act
            var result = controller.Delete(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            // Arrange
            var itemcardapio = new Itemcardapio { Id = 1, Nome = "Feijoada" };
            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);

            // Act
            var result = controller.DeleteConfirmed(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Delete(1), Times.Once);
        }

        [TestMethod()]
        public void DeleteConfirmedTest_ItemNaoEncontrado()
        {
            // Arrange
            mockService.Setup(s => s.Get(999)).Returns((Itemcardapio)null);

            // Act
            var result = controller.DeleteConfirmed(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void GetIngredientesUnicosTest()
        {
            // Arrange
            var ingredientes = new List<string> { "Arroz", "Feijão", "Carne" };
            mockService.Setup(s => s.GetAllIngredientes()).Returns(ingredientes);

            // Act
            var result = controller.GetIngredientesUnicos() as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            var dados = result.Value as IEnumerable<string>;
            Assert.IsNotNull(dados);
            Assert.AreEqual(3, dados.Count());
        }

        [TestMethod()]
        public void GetIngredientesUnicosTest_ErroInterno()
        {
            // Arrange
            mockService.Setup(s => s.GetAllIngredientes()).Throws(new System.Exception("Erro no banco"));

            // Act
            var result = controller.GetIngredientesUnicos() as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);
            Assert.IsTrue(result.Value.ToString().Contains("erro interno"));
        }
    }
}