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
    public class MesaControllerTests
    {
        private Mock<IMesaService> mockMesaService;
        private Mock<IMapper> mockMapper;
        private MesaController controller;

        private IEnumerable<Mesa> testMesas;
        private IEnumerable<MesaViewModel> testMesasModel;
        private Mesa targetMesa;
        private MesaViewModel targetMesaModel;

        [TestInitialize]
        public void Setup()
        {
            mockMesaService = new Mock<IMesaService>();
            mockMapper = new Mock<IMapper>();
            controller = new MesaController(mockMesaService.Object, mockMapper.Object);

            testMesas = GerarMesas();
            testMesasModel = GerarMesasViewModel();
            targetMesa = GerarMesa();
            targetMesaModel = GerarMesaViewModel();

            mockMapper.Setup(m => m.Map<IEnumerable<MesaViewModel>>(testMesas)).Returns(testMesasModel);
            mockMapper.Setup(m => m.Map<MesaViewModel>(targetMesa)).Returns(targetMesaModel);
            mockMapper.Setup(m => m.Map<Mesa>(targetMesaModel)).Returns(targetMesa);

            mockMesaService.Setup(s => s.Get(1)).Returns(targetMesa);
            mockMesaService.Setup(s => s.GetAll()).Returns(testMesas);
            mockMesaService.Setup(s => s.Create(It.IsAny<Mesa>())).Verifiable();
            mockMesaService.Setup(s => s.Edit(It.IsAny<Mesa>())).Verifiable();
            mockMesaService.Setup(s => s.Delete(It.IsAny<int>())).Verifiable();
        }

        private IEnumerable<Mesa> GerarMesas() => new List<Mesa> {
            new Mesa { Id = 1, Imagem = new byte[] { 1, 2, 3 } },
            new Mesa { Id = 2, Imagem = new byte[] { 4, 5, 6 } }
        };

        private IEnumerable<MesaViewModel> GerarMesasViewModel() => new List<MesaViewModel> {
            new MesaViewModel { Id = 1, Imagem = new byte[] { 1, 2, 3 } },
            new MesaViewModel { Id = 2, Imagem = new byte[] { 4, 5, 6 } }
        };

        private Mesa GerarMesa() => new Mesa { Id = 1, Imagem = new byte[] { 1, 2, 3 } };

        private MesaViewModel GerarMesaViewModel() => new MesaViewModel { Id = 1, Imagem = new byte[] { 1, 2, 3 } };

        [TestMethod]
        public void Index_DeveRetornarViewComMesas()
        {
            var result = controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<MesaViewModel>));
            List<MesaViewModel>? lista = viewResult.ViewData.Model as List<MesaViewModel>;
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
            var result = controller.Create(targetMesaModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Edit_Get_DeveRetornarViewComMesa()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MesaViewModel));
            MesaViewModel? mesaModel = viewResult.ViewData.Model as MesaViewModel;
            Assert.IsNotNull(mesaModel);
            Assert.AreEqual(1, mesaModel.Id);
        }

        [TestMethod]
        public void Edit_Post_ModelValida_DeveRedirecionarParaIndex()
        {
            var result = controller.Edit(targetMesaModel.Id, targetMesaModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }

        [TestMethod]
        public void Delete_Get_DeveRetornarViewComMesa()
        {
            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(MesaViewModel));
            MesaViewModel? mesaModel = viewResult.ViewData.Model as MesaViewModel;
            Assert.IsNotNull(mesaModel);
            Assert.AreEqual(1, mesaModel.Id);
        }

        [TestMethod]
        public void Delete_Post_DeveRedirecionarParaIndex()
        {
            var result = controller.Delete(targetMesaModel);

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirect = result as RedirectToActionResult;
            Assert.IsNull(redirect.ControllerName);
            Assert.AreEqual("Index", redirect.ActionName);
        }
    }
}
