using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestauranteWeb.Controllers;
using Moq;
using AutoMapper;
using Core.Service;
using Core;
using RestauranteAkiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace RestauranteWeb.Controllers.Tests
{
    [TestClass()]
    public class PedidoitemcardapioControllerTests
    {
        private Mock<IPedidoItemcardapioService> mockService;
        private Mock<IMapper> mockMapper;
        private PedidoitemcardapioController controller;

        [TestInitialize]
        public void Setup()
        {
            mockService = new Mock<IPedidoItemcardapioService>();
            mockMapper = new Mock<IMapper>();
            controller = new PedidoitemcardapioController(mockService.Object, mockMapper.Object);
        }

        [TestMethod()]
        public void PedidoitemcardapioControllerTest()
        {
            // Arrange & Act
            var controller = new PedidoitemcardapioController(mockService.Object, mockMapper.Object);

            // Assert
            Assert.IsNotNull(controller);
        }

        [TestMethod()]
        public void IndexTest()
        {
            // Arrange
            var listaPedidoItemCardapio = new List<PedidoItemcardapio>
            {
                new PedidoItemcardapio { IdPedido = 1, IdItemCardapio = 1, Quantidade = 2 },
                new PedidoItemcardapio { IdPedido = 2, IdItemCardapio = 2, Quantidade = 1 }
            };

            var listaViewModel = new List<PedidoItemcardapioViewModel>
            {
                new PedidoItemcardapioViewModel { IdPedido = 1, IdItemCardapio = 1, Quantidade = 2 },
                new PedidoItemcardapioViewModel { IdPedido = 2, IdItemCardapio = 2, Quantidade = 1 }
            };

            mockService.Setup(s => s.GetAll()).Returns(listaPedidoItemCardapio);
            mockMapper.Setup(m => m.Map<List<PedidoItemcardapioViewModel>>(listaPedidoItemCardapio))
                      .Returns(listaViewModel);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(List<PedidoItemcardapioViewModel>));
            var model = result.Model as List<PedidoItemcardapioViewModel>;
            Assert.AreEqual(2, model.Count);
            mockService.Verify(s => s.GetAll(), Times.Once);
        }

        [TestMethod()]
        public void CreateTest()
        {
            // Act
            var result = controller.Create() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void CreateTest1()
        {
            // Arrange
            var viewModel = new PedidoItemcardapioViewModel
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 3
            };

            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 3
            };

            mockMapper.Setup(m => m.Map<PedidoItemcardapio>(viewModel)).Returns(pedidoItem);
            mockService.Setup(s => s.Create(pedidoItem)).Returns(1);

            // Act
            var result = controller.Create(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Create(It.IsAny<PedidoItemcardapio>()), Times.Once);
        }

        [TestMethod()]
        public void EditTest()
        {
            // Arrange
            int idPedido = 1;
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            mockMapper.Setup(m => m.Map<PedidoItemcardapio>(idPedido)).Returns(pedidoItem);

            // Act
            var result = controller.Edit(idPedido) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod()]
        public void EditTest1()
        {
            // Arrange
            var viewModel = new PedidoItemcardapioViewModel
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 5
            };

            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 5
            };

            mockMapper.Setup(m => m.Map<PedidoItemcardapio>(viewModel)).Returns(pedidoItem);

            // Act
            var result = controller.Edit(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Edit(It.IsAny<PedidoItemcardapio>()), Times.Once);
        }

        [TestMethod()]
        public void DetailsTest()
        {
            // Arrange
            int idPedido = 1;
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            var viewModel = new PedidoItemcardapioViewModel
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            mockService.Setup(s => s.Get(idPedido)).Returns(pedidoItem);
            mockMapper.Setup(m => m.Map<PedidoItemcardapioViewModel>(pedidoItem)).Returns(viewModel);

            // Act
            var result = controller.Details(idPedido) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(PedidoItemcardapioViewModel));
            var model = result.Model as PedidoItemcardapioViewModel;
            Assert.AreEqual(1, model.IdPedido);
            mockService.Verify(s => s.Get(idPedido), Times.Once);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Arrange
            int idPedido = 1;
            var pedidoItem = new PedidoItemcardapio
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            var viewModel = new PedidoItemcardapioViewModel
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            mockService.Setup(s => s.Get(idPedido)).Returns(pedidoItem);
            mockMapper.Setup(m => m.Map<PedidoItemcardapioViewModel>(pedidoItem)).Returns(viewModel);

            // Act
            var result = controller.Delete(idPedido) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(PedidoItemcardapioViewModel));
            mockService.Verify(s => s.Get(idPedido), Times.Once);
        }

        [TestMethod()]
        public void DeleteTest1()
        {
            // Arrange
            int idPedido = 1;
            var viewModel = new PedidoItemcardapioViewModel
            {
                IdPedido = 1,
                IdItemCardapio = 1,
                Quantidade = 2
            };

            // Act
            var result = controller.Delete(idPedido, viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Delete(idPedido), Times.Once);
        }

        [TestMethod()]
        public void DetailsTest_PedidoNaoEncontrado()
        {
            // Arrange
            int idPedido = 999;
            mockService.Setup(s => s.Get(idPedido)).Returns((PedidoItemcardapio)null);
            mockMapper.Setup(m => m.Map<PedidoItemcardapioViewModel>(null))
                      .Returns((PedidoItemcardapioViewModel)null);

            // Act
            var result = controller.Details(idPedido) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);
        }

        [TestMethod()]
        public void CreateTest1_ModelStateInvalido()
        {
            // Arrange
            var viewModel = new PedidoItemcardapioViewModel
            {
                IdItemCardapio = 1,
                Quantidade = 3
                // IdPedido ausente para tornar ModelState inválido
            };

            controller.ModelState.AddModelError("IdPedido", "O campo Pedido é obrigatório.");

            // Act
            var result = controller.Create(viewModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Create(It.IsAny<PedidoItemcardapio>()), Times.Never);
        }
    }
}