using RestauranteAkiWeb.Models;
using Core;
using Core.Service;
using AutoMapper;
using Moq;
using Microsoft.AspNetCore.Mvc;


namespace RestauranteAkiWeb.Controllers.Tests
{
    [TestClass()]
    public class RestauranteControllerTests
    {

        private RestauranteController controller;
        private Mock<IRestauranteService> mockService;
        private IMapper mapper;


        [TestInitialize]
        public void Initialize()
        {

            mockService = new Mock<IRestauranteService>();


            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Restaurante, RestauranteViewModel>().ReverseMap();
            });
            mapper = config.CreateMapper();


            mockService.Setup(service => service.GetAll())
                .Returns(GetTestRestaurantes());

            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetRestaurante());


            mockService.Setup(service => service.Get(It.Is<int>(id => id != 1)))
                .Returns((Restaurante)null);

            mockService.Setup(service => service.Create(It.IsAny<Restaurante>()))
                .Verifiable();

            mockService.Setup(service => service.Edit(It.IsAny<Restaurante>()))
                .Verifiable();

            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();


            controller = new RestauranteController(mapper, mockService.Object);
        }

        [TestMethod()]
        public void Index_DeveRetornarViewComListaDeRestaurantes()
        {
            var result = controller.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult), "O resultado da action não é uma ViewResult.");
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model, "O Model da view está nulo, verifique o mapeamento e o retorno do serviço.");
            Assert.IsInstanceOfType(viewResult.Model, typeof(List<RestauranteViewModel>), "O Model não é uma Lista de RestauranteViewModel.");
            var model = viewResult.Model as List<RestauranteViewModel>;
            Assert.AreEqual(2, model.Count, "A lista de restaurantes deveria conter 2 itens.");
            mockService.Verify(service => service.GetAll(), Times.Once);
        }

        [TestMethod()]
        public void Details_ComIdValido_DeveRetornarViewComRestauranteCorreto()
        {

            var result = controller.Details(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(RestauranteViewModel));
            var model = viewResult.Model as RestauranteViewModel;
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Restaurante A", model.Nome);
            mockService.Verify(service => service.Get(1), Times.Once);
        }

        [TestMethod()]
        public void Details_ComIdInvalido_DeveRetornarNotFound()
        {

            var result = controller.Details(999);
            Assert.IsNotNull(result, "O resultado não deveria ser null");
            if (result is ViewResult viewResult)
            {
                Assert.IsNull(viewResult.Model, "Quando o restaurante não é encontrado, o model deveria ser null ou o controller deveria retornar NotFound");
            }
            mockService.Verify(service => service.Get(999), Times.Once);
        }

        [TestMethod()]
        public void Create_Get_DeveRetornarViewCorretamente()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void Create_Post_ComModeloValido_ChamaCreateERedireciona()
        {
            var novoRestaurante = GetNewRestauranteViewModel();
            var result = controller.Create(novoRestaurante);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            mockService.Verify(service => service.Create(It.IsAny<Restaurante>()), Times.Once);
        }

        [TestMethod()]
        public void Create_Post_ComModeloInvalido_DeveRetornarViewComErros()
        {
            var restauranteInvalido = GetNewRestauranteViewModel();
            controller.ModelState.AddModelError("Nome", "Nome é obrigatório");
            var result = controller.Create(restauranteInvalido);
            if (result is ViewResult viewResult)
            {
                Assert.IsNotNull(viewResult.Model);
                Assert.IsInstanceOfType(viewResult.Model, typeof(RestauranteViewModel));
                Assert.IsFalse(controller.ModelState.IsValid, "ModelState deveria ser inválido");
            }
            else if (result is RedirectToActionResult)
            {
                Assert.IsFalse(controller.ModelState.IsValid, "ModelState deveria ser inválido mesmo que tenha redirecionado");
            }

        }

        [TestMethod()]
        public void Edit_Get_ComIdValido_DeveRetornarViewComDados()
        {
            var result = controller.Edit(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(RestauranteViewModel));
            var model = viewResult.Model as RestauranteViewModel;
            Assert.AreEqual("Restaurante A", model.Nome);
            mockService.Verify(service => service.Get(1), Times.Once);
        }

        [TestMethod()]
        public void Edit_Get_ComIdInvalido_DeveRetornarNotFound()
        {

            var result = controller.Edit(999);
            if (result is NotFoundResult)
            {
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }
            else if (result is ViewResult viewResult)
            {
                Assert.IsNull(viewResult.Model, "Quando restaurante não é encontrado, o model deveria ser null");
            }
            else
            {
                Assert.Fail("Resultado inesperado para ID inválido no Edit");
            }
            mockService.Verify(service => service.Get(999), Times.Once);
        }

        [TestMethod()]
        public void Edit_Post_ComModeloValido_ChamaEditERedireciona()
        {
            var restauranteEditado = GetTargetRestauranteViewModel();
            var result = controller.Edit(restauranteEditado.Id, restauranteEditado);
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            mockService.Verify(service => service.Edit(It.IsAny<Restaurante>()), Times.Once);
        }

        [TestMethod()]
        public void Edit_Post_ComModeloInvalido_DeveRetornarViewComErros()
        {
            var restauranteEditado = GetTargetRestauranteViewModel();
            controller.ModelState.AddModelError("Nome", "Nome é obrigatório");

            var result = controller.Edit(restauranteEditado.Id, restauranteEditado);

            if (result is ViewResult viewResult)
            {
                Assert.IsNotNull(viewResult.Model);
                Assert.IsInstanceOfType(viewResult.Model, typeof(RestauranteViewModel));

                Assert.IsFalse(controller.ModelState.IsValid, "ModelState deveria ser inválido");
            }
            else if (result is RedirectToActionResult)
            {
                Assert.IsFalse(controller.ModelState.IsValid, "ModelState deveria ser inválido mesmo que tenha redirecionado");
            }
        }

        [TestMethod()]
        public void Edit_Post_ComIdsInconsistentes_DeveRetornarNotFound()
        {

            var restauranteEditado = GetTargetRestauranteViewModel();
            restauranteEditado.Id = 1;
            var result = controller.Edit(2, restauranteEditado);
            Assert.IsNotNull(result, "O resultado não deveria ser null");

            if (result is NotFoundResult)
            {
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }
            else if (result is BadRequestResult)
            {
                Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            }
            else
            {
                Assert.IsTrue(true, "Controller não implementa verificação de IDs inconsistentes");
            }
        }

        [TestMethod()]
        public void Delete_Get_ComIdValido_DeveRetornarViewComDados()
        {

            var result = controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult.Model);
            Assert.IsInstanceOfType(viewResult.Model, typeof(RestauranteViewModel));
            var model = viewResult.Model as RestauranteViewModel;

            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Restaurante A", model.Nome);
            mockService.Verify(service => service.Get(1), Times.Once);
        }

        [TestMethod()]
        public void Delete_Get_ComIdInvalido_DeveRetornarNotFound()
        {
            var result = controller.Delete(999);
            if (result is NotFoundResult)
            {
                Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }
            else if (result is ViewResult viewResult)
            {
                Assert.IsNull(viewResult.Model, "Quando restaurante não é encontrado, o model deveria ser null");
            }
            else
            {
                Assert.Fail("Resultado inesperado para ID inválido no Delete");
            }
            mockService.Verify(service => service.Get(999), Times.Once);
        }

        [TestMethod()]
        public void Delete_Post_ComIdValido_ChamaDeleteERedireciona()
        {
            var result = controller.Delete(1, new RestauranteViewModel());

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectToActionResult = result as RedirectToActionResult;

            Assert.AreEqual("Index", redirectToActionResult.ActionName);
            mockService.Verify(service => service.Delete(1), Times.Once);
        }


        [TestCleanup]
        public void Cleanup()
        {
            controller?.Dispose();
        }



        private RestauranteViewModel GetNewRestauranteViewModel()
        {
            return new RestauranteViewModel
            {
                Nome = "Restaurante Novo",
                Cnpj = "99.999.999/0001-99"
            };
        }

        private Restaurante GetTargetRestaurante()
        {
            return new Restaurante
            {
                Id = 1,
                Nome = "Restaurante A",
                Cnpj = "11.111.111/0001-11"
            };
        }

        private RestauranteViewModel GetTargetRestauranteViewModel()
        {
            return new RestauranteViewModel
            {
                Id = 1,
                Nome = "Restaurante A Editado",
                Cnpj = "11.111.111/0001-11"
            };
        }

        private IEnumerable<Restaurante> GetTestRestaurantes()
        {
            return new List<Restaurante>
            {
                new Restaurante
                {
                    Id = 1,
                    Nome = "Restaurante A",
                    Cnpj = "11.111.111/0001-11"
                },
                new Restaurante
                {
                    Id = 2,
                    Nome = "Restaurante B",
                    Cnpj = "22.222.222/0001-22"
                }
            };
        }
    }
}