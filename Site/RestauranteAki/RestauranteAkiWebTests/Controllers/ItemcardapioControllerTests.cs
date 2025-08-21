using AutoMapper;
using Core;
using Core.Service;
using Moq;
using RestauranteAkiWeb.Controllers;
using RestauranteAkiWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace RestauranteAkiWeb.Tests.Controllers
{
    [TestClass]
    public class ItemcardapioControllerTests
    {
        private static ItemcardapioController controller;

        [TestInitialize]
        public void Initialize()
        {
            var mockService = new Mock<IItemcardapioService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.CreateMap<Itemcardapio, ItemcardapioViewModel>().ReverseMap()).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestItens());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetItem());
            mockService.Setup(service => service.Create(It.IsAny<Itemcardapio>()))
                .Verifiable();
            mockService.Setup(service => service.Edit(It.IsAny<Itemcardapio>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new ItemcardapioController(mapper, mockService.Object);
        }

        [TestMethod]
        public void IndexTest_Valido()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<ItemcardapioViewModel>));

            var lista = (List<ItemcardapioViewModel>)viewResult.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod]
        public void DetailsTest_Valido()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(ItemcardapioViewModel));

            var model = (ItemcardapioViewModel)viewResult.Model;
            Assert.AreEqual("Pizza", model.Nome);
            Assert.AreEqual(25.0, model.PrecoUnitario);
        }

        [TestMethod]
        public void CreateTest_Get_Valido()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CreateTest_Post_Valido()
        {
            var result = controller.Create(GetNewItem());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void CreateTest_Post_Invalido()
        {
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            var result = controller.Create(GetNewItem());

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditTest_Get_Valido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(ItemcardapioViewModel));

            var model = (ItemcardapioViewModel)viewResult.Model;
            Assert.AreEqual("Pizza", model.Nome);
        }

        [TestMethod]
        public void EditTest_Post_Valido()
        {
            var model = GetTargetItemViewModel();
            var result = controller.Edit(model.Id, model);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void DeleteTest_Get_Valido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = (ViewResult)result;
            var model = (ItemcardapioViewModel)viewResult.Model;
            Assert.AreEqual("Pizza", model.Nome);
        }

        [TestMethod]
        public void DeleteTest_Post_Valido()
        {
            var model = GetTargetItemViewModel();
            var result = controller.Delete(model.Id, model);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = (RedirectToActionResult)result;
            Assert.AreEqual("Index", redirect.ActionName);
        }

        private ItemcardapioViewModel GetNewItem()
        {
            return new ItemcardapioViewModel
            {
                Id = 4,
                Nome = "Hamburguer",
                PrecoUnitario = 15
            };
        }

        private static Itemcardapio GetTargetItem()
        {
            return new Itemcardapio
            {
                Id = 1,
                Nome = "Pizza",
                PrecoUnitario = 25
            };
        }

        private ItemcardapioViewModel GetTargetItemViewModel()
        {
            return new ItemcardapioViewModel
            {
                Id = 1,
                Nome = "Pizza",
                PrecoUnitario = 25
            };
        }

        private IEnumerable<Itemcardapio> GetTestItens()
        {
            return new List<Itemcardapio>
            {
                new Itemcardapio { Id = 1, Nome = "Pizza", PrecoUnitario = 25 },
                new Itemcardapio { Id = 2, Nome = "Coxinha", PrecoUnitario = 5 },
                new Itemcardapio { Id = 3, Nome = "Refrigerante", PrecoUnitario = 7 }
            };
        }
    }
}
