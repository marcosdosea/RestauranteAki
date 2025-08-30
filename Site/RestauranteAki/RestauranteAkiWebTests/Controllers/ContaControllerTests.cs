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
    public class ContumControllerTests
    {
        private Mock<IContumService> mockContumService;
        private Mock<IMapper> mockMapper;
        private ContaController controller;

        private IEnumerable<Contum> testContas;
        private IEnumerable<ContumViewModel> testContasModel;
        private Contum targetConta;
        private ContumViewModel targetContaModel;

        [TestInitialize]
        public void Setup()
        {
            mockContumService = new Mock<IContumService>();
            mockMapper = new Mock<IMapper>();
            controller = new ContaController(mockContumService.Object, mockMapper.Object);

            testContas = GerarContas();
            testContasModel = GerarContasViewModel();
            targetConta = GerarConta();
            targetContaModel = GerarContaViewModel();

            mockMapper.Setup(m => m.Map<IEnumerable<ContumViewModel>>(testContas)).Returns(testContasModel);
            mockMapper.Setup(m => m.Map<ContumViewModel>(targetConta)).Returns(targetContaModel);
            mockMapper.Setup(m => m.Map<Contum>(targetContaModel)).Returns(targetConta);

            mockContumService.Setup(s => s.Get(1)).Returns(targetConta);
            mockContumService.Setup(s => s.GetAll()).Returns(testContas);
            mockContumService.Setup(s => s.Create(It.IsAny<Contum>())).Verifiable();
            mockContumService.Setup(s => s.Edit(It.IsAny<Contum>())).Verifiable();
            mockContumService.Setup(s => s.Delete(It.IsAny<int>())).Verifiable();
        }

        private IEnumerable<Contum> GerarContas() => new List<Contum> {
            new Contum { Id = 1, Valor = 100, Status = "A", FormaPagamento = "Dinheiro", IdMesa = 1 },
            new Contum { Id = 2, Valor = 200, Status = "F", FormaPagamento = "Cartão", IdMesa = 2 }
        };

        private IEnumerable<ContumViewModel> GerarContasViewModel() => new List<ContumViewModel> {
            new ContumViewModel { Id = 1, Valor = 100, Status = "A", FormaPagamento = "Dinheiro", IdMesa = 1 },
            new ContumViewModel { Id = 2, Valor = 200, Status = "F", FormaPagamento = "Cartão", IdMesa = 2 }
        };

        private Contum GerarConta() => new Contum { Id = 1, Valor = 150, Status = "A", FormaPagamento = "Pix", IdMesa = 1 };

        private ContumViewModel GerarContaViewModel() => new ContumViewModel { Id = 1, Valor = 150, Status = "A", FormaPagamento = "Pix", IdMesa = 1 };

        [TestMethod]
        public void Index_DeveRetornarViewComContas()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<ContumViewModel>));
            List<ContumViewModel>? lista = viewResult.ViewData.Model as List<ContumViewModel>;
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
            var result = controller.Create(targetContaModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Create_Post_Invalido()
        {
            controller.ModelState.AddModelError("IdMesa", "Campo requerido");

            var result = controller.Create(targetContaModel);

            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Edit_Get_DeveRetornarViewComConta()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ContumViewModel));
            ContumViewModel? contaModel = viewResult.ViewData.Model as ContumViewModel;
            Assert.IsNotNull(contaModel);
            Assert.AreEqual(1, contaModel.Id);
            Assert.AreEqual("A", contaModel.Status);
        }

        [TestMethod]
        public void Edit_Post_ModelValida_DeveRedirecionarParaIndex()
        {
            var result = controller.Edit(targetContaModel.Id, targetContaModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Delete_Post_DeveRedirecionarParaIndex()
        {
            var result = controller.Delete(targetContaModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Delete_Get_DeveRetornarViewComConta()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ContumViewModel));
            ContumViewModel? contaModel = viewResult.ViewData.Model as ContumViewModel;
            Assert.IsNotNull(contaModel);
            Assert.AreEqual(1, contaModel.Id);
            Assert.AreEqual("A", contaModel.Status);
        }
    }
}
