using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RestauranteAkiWeb.Mappers;
using RestauranteAkiWeb.Models;

namespace RestauranteAkiWeb.Controllers.Tests
{
    [TestClass()]
    public class PessoaControllerTests
    {
        private static PessoaController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IPessoaService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PessoaProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestPessoas());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetPessoa());
            mockService.Setup(service => service.Edit(It.IsAny<Pessoa>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Pessoa>()))
                .Verifiable();
            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new PessoaController(mockService.Object, null, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.IndexGestor(); // Consertar isso depois

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(List<PessoaViewModel>));

            List<PessoaViewModel>? lista = (List<PessoaViewModel>)viewResult.ViewData.Model;
            Assert.AreEqual(3, lista.Count);
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));
            PessoaViewModel pessoaModel = (PessoaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("João Silva", pessoaModel.NomeCompleto);
            Assert.AreEqual(TipoPessoa.Gestor, pessoaModel.TipoPessoa);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            // Act
            var result = controller.Create(new PessoaViewModel { });

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post_Valido()
        {
            // Act
            var result = controller.Create(GetNewPessoa());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalido()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller.Create(GetNewPessoa());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void EditTest_Get_Valido()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));
            PessoaViewModel pessoaModel = (PessoaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("João Silva", pessoaModel.NomeCompleto);
            Assert.AreEqual(TipoPessoa.Gestor, pessoaModel.TipoPessoa);
        }

        [TestMethod()]
        public void EditTest_Post_Valido()
        {
            // Act
            var result = controller.Edit(GetTargetPessoaModel().Id, GetTargetPessoaModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [TestMethod()]
        public void EditTest_Post_IdInvalido()
        {
            // Arrange
            var pessoaModel = GetTargetPessoaModel();

            // Act
            var result = controller.Edit(999, pessoaModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public void DeleteTest_Get_Valido()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaViewModel));
            PessoaViewModel pessoaModel = (PessoaViewModel)viewResult.ViewData.Model;
            Assert.AreEqual("João Silva", pessoaModel.NomeCompleto);
        }

        [TestMethod()]
        public void DeleteTest_Post_Valido()
        {
            // Act
            var result = controller.Delete(GetTargetPessoaModel().Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        #region Métodos Auxiliares

        private PessoaViewModel GetNewPessoa()
        {
            return new PessoaViewModel
            {
                Id = 4,
                NomeCompleto = "Carlos Pereira",
                TipoPessoa = TipoPessoa.Gestor,
                IdRestaurante = 1
            };
        }

        private static Pessoa GetTargetPessoa()
        {
            return new Pessoa
            {
                Id = 1,
                NomeCompleto = "João Silva",
                TipoPessoa = "G",
                IdRestaurante = 1
            };
        }

        private PessoaViewModel GetTargetPessoaModel()
        {
            return new PessoaViewModel
            {
                Id = 1,
                NomeCompleto = "João Silva",
                TipoPessoa = TipoPessoa.Gestor,
                IdRestaurante = 1
            };
        }

        private IEnumerable<Pessoa> GetTestPessoas()
        {
            return new List<Pessoa>
            {
                new Pessoa { Id = 1, NomeCompleto = "João Silva", TipoPessoa = "G", IdRestaurante = 1 },
                new Pessoa { Id = 2, NomeCompleto = "Maria Souza", TipoPessoa = "G", IdRestaurante = 1 },
                new Pessoa { Id = 3, NomeCompleto = "Pedro Santos", TipoPessoa = "G", IdRestaurante = 1 }
            };
        }

        #endregion
    }
}
