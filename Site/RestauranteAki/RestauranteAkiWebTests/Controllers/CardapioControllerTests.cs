using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestauranteAkiWeb.Mappers;
using RestauranteAkiWeb.Models;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RestauranteAkiWeb.Controllers.Tests
{
    [TestClass()]
    public class CardapioControllerTests
    {
        private static CardapioController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange - configuração inicial dos mocks e mapper
            var mockService = new Mock<ICardapioService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new CardapioProfile())).CreateMapper();

            // Setup do mock com comportamentos pré-definidos
            mockService.Setup(service => service.GetAll())
                .Returns(GetTestCardapios());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetCardapio());
            mockService.Setup(service => service.Edit(It.IsAny<Cardapio>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Cardapio>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new CardapioController(mapper, mockService.Object);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<CardapioViewModel>));

            var lista = (List<CardapioViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CardapioViewModel));
            var cardapioModel = (CardapioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardápio de Verão", cardapioModel.Nome);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            var result = controller.Create(GetNewCardapio());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "Campo requerido");
            var model = GetNewCardapio();

            // Act
            var result = controller.Create(model);

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CardapioViewModel));
            var cardapioModel = (CardapioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardápio de Verão", cardapioModel.Nome);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            var model = GetTargetCardapioModel();
            var result = controller.Edit(model.Id, model);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod()]
        public void EditTest_Post_IdInvalido()
        {
            var model = GetTargetCardapioModel();
            var result = controller.Edit(999, model);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(CardapioViewModel));
            var cardapioModel = (CardapioViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("Cardápio de Verão", cardapioModel.Nome);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            var model = GetTargetCardapioModel();
            var result = controller.Delete(model.Id, model);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        #region Métodos Auxiliares

        private CardapioViewModel GetNewCardapio()
        {
            return new CardapioViewModel
            {
                Id = 4,
                Nome = "Cardápio de Outono",
                DataInicio = DateTime.Now,
                DataFim = DateTime.Now.AddMonths(2),
                Ativo = 1,
                IdRestaurante = 1
            };
        }

        private static Cardapio GetTargetCardapio()
        {
            return new Cardapio
            {
                Id = 1,
                Nome = "Cardápio de Verão",
                DataInicio = new DateTime(2025, 1, 1),
                DataFim = new DateTime(2025, 3, 31),
                Ativo = 1,
                IdRestaurante = 1
            };
        }

        private CardapioViewModel GetTargetCardapioModel()
        {
            return new CardapioViewModel
            {
                Id = 1,
                Nome = "Cardápio de Verão",
                DataInicio = new DateTime(2025, 1, 1),
                DataFim = new DateTime(2025, 3, 31),
                Ativo = 1,
                IdRestaurante = 1
            };
        }

        private IEnumerable<Cardapio> GetTestCardapios()
        {
            return new List<Cardapio>
            {
                new Cardapio { Id = 1, Nome = "Cardápio de Verão", DataInicio = DateTime.Now.AddDays(-10), DataFim = DateTime.Now.AddMonths(2), Ativo = 1, IdRestaurante = 1 },
                new Cardapio { Id = 2, Nome = "Cardápio de Inverno", DataInicio = DateTime.Now.AddMonths(-3), DataFim = DateTime.Now.AddMonths(-1), Ativo = 0, IdRestaurante = 1 },
                new Cardapio { Id = 3, Nome = "Cardápio Especial", DataInicio = DateTime.Now, DataFim = DateTime.Now.AddMonths(1), Ativo = 1, IdRestaurante = 1 }
            };
        }
        public ActionResult Details(int id)
        {
            var cardapio = cardapioService.Get(id);
            if (cardapio == null)
                return NotFound();
            var cardapioViewModel = mapper.Map<CardapioViewModel>(cardapio);
            return View(cardapioViewModel);
        }
        #endregion
    }
}
