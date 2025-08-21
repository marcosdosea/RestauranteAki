using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestauranteAkiWeb.Controllers;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWebTests.Controllers
{
    [TestClass]
    public class PedidoControllerTests
    {
        private Mock<IPedidoService> mockPedidoService;
        private Mock<IMapper> mockMapper;
        private PedidoController controller;

        private IEnumerable<Pedido> testPedidos;
        private IEnumerable<PedidoViewModel> testPedidosModel;
        private Pedido targetPedido;
        private PedidoViewModel targetPedidoModel;

        [TestInitialize]
        public void Setup()
        {
            mockPedidoService = new Mock<IPedidoService>();
            mockMapper = new Mock<IMapper>();
            controller = new PedidoController(mockPedidoService.Object, mockMapper.Object);

            testPedidos = GerarPedidos();
            testPedidosModel = GerarPedidosViewModel();
            targetPedido = GerarPedido();
            targetPedidoModel = GerarPedidoViewModel();

            mockMapper.Setup(m => m.Map<IEnumerable<PedidoViewModel>>(testPedidos)).Returns(testPedidosModel);
            mockMapper.Setup(m => m.Map<PedidoViewModel>(targetPedido)).Returns(targetPedidoModel);
            mockMapper.Setup(m => m.Map<Pedido>(targetPedidoModel)).Returns(targetPedido);

            mockPedidoService.Setup(s => s.Get(1)).Returns(targetPedido);
            mockPedidoService.Setup(s => s.GetAll()).Returns(testPedidos);
            mockPedidoService.Setup(s => s.Create(It.IsAny<Pedido>())).Verifiable();
            mockPedidoService.Setup(s => s.Edit(It.IsAny<Pedido>())).Verifiable();
        }

        private IEnumerable<Pedido> GerarPedidos() => new List<Pedido> {
            new Pedido { Id = 1, Status = "S" },
            new Pedido { Id = 2, Status = "P" }
        };

        private IEnumerable<PedidoViewModel> GerarPedidosViewModel() => new List<PedidoViewModel> {
            new PedidoViewModel { Id = 1, Status = "S" },
            new PedidoViewModel { Id = 2, Status = "P" }
        };

        private Pedido GerarPedido() => new Pedido { Id = 1, Status = "S", IdMesa = 3 };

        private PedidoViewModel GerarPedidoViewModel() => new PedidoViewModel { Id = 1, Status = "S", IdMesa = 2 };

        [TestMethod]
        public void Index_DeveRetornarViewComPedidos()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<PedidoViewModel>));
            List<PedidoViewModel>? lista = viewResult.ViewData.Model as List<PedidoViewModel>;
            Assert.IsNotNull(lista);
            Assert.AreEqual(2, lista.Count);
        }

        [TestMethod]
        public void Create_Get_DeveRetornarView()
        {
            var result = controller.Create();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Create_Post_ModelValida_DeveRedirecionarParaIndex()
        {
            var result = controller.Create(targetPedidoModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Create_Post_Invalido()
        {
            controller.ModelState.AddModelError("IdMesa", "Campo requerido");

            var result = controller.Create(targetPedidoModel);

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Edit_Get_DeveRetornarViewComPedido()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PedidoViewModel));
            PedidoViewModel? pedidoModel = viewResult.ViewData.Model as PedidoViewModel;
            Assert.IsNotNull(pedidoModel);
            Assert.AreEqual(1, pedidoModel.Id);
            Assert.AreEqual("S", pedidoModel.Status);
        }

        [TestMethod]
        public void Edit_Post_ModelValida_DeveRedirecionarParaIndex()
        {
            var result = controller.Edit(targetPedidoModel.Id, targetPedidoModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Delete_Post_DeveRedirecionarParaIndex()
        {
            var result = controller.Delete(targetPedidoModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Delete_Get_DeveRetornarViewComPedido()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PedidoViewModel));
            PedidoViewModel? pedidoModel = viewResult.ViewData.Model as PedidoViewModel;
            Assert.IsNotNull(pedidoModel);
            Assert.AreEqual(1, pedidoModel.Id);
            Assert.AreEqual("S", pedidoModel.Status);
        }
    }
}
