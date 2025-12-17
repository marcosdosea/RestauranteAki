using RestauranteAkiWeb.Models;
using Core;
using Core.Service;
using AutoMapper;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace RestauranteAkiWeb.Controllers.Tests
{
    [TestClass]
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

        [TestMethod]
        public void ItemcardapioControllerTest()
        {
            Assert.IsNotNull(controller);
        }

        [TestMethod]
        public void IndexTest()
        {
            var itemcardapios = GetTestItemcardapios().ToList();
            var viewModels = itemcardapios.Select(i => new ItemcardapioViewModel { Id = i.Id, Nome = i.Nome }).ToList();

            mockService.Setup(s => s.GetAll()).Returns(itemcardapios);
            mockMapper.Setup(m => m.Map<List<ItemcardapioViewModel>>(itemcardapios)).Returns(viewModels);

            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            var model = result.Model as List<ItemcardapioViewModel>;
            Assert.AreEqual(3, model.Count);
        }

        [TestMethod]
        public void DetailsTest()
        {
            var itemcardapio = GetTargetItemcardapio();
            var viewModel = new ItemcardapioViewModel { Id = 1, Nome = "Hambúrguer X" };

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map<ItemcardapioViewModel>(itemcardapio)).Returns(viewModel);

            var result = controller.Details(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Hambúrguer X", ((ItemcardapioViewModel)result.Model).Nome);
        }

        [TestMethod]
        public void CreateGetTest()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CreatePostTest_ModeloValido()
        {
            var model = GetCadastroCompletoModel();
            var entidade = GetTargetItemcardapio();

            mockMapper.Setup(m => m.Map<Itemcardapio>(model)).Returns(entidade);
            mockService.Setup(s => s.Create(It.IsAny<Itemcardapio>())).Returns(1);

            var result = controller.Create(model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
        }

        [TestMethod]
        public void CreatePostTest_ModeloInvalido()
        {
            controller.ModelState.AddModelError("Descricao", "É necessário informar ao menos 1 ingrediente.");

            var result = controller.Create(GetCadastroCompletoModel());

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditGetTest()
        {
            var itemcardapio = GetTargetItemcardapio();
            var viewModel = GetTargetItemcardapioModel();

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map<ItemcardapioViewModel>(itemcardapio)).Returns(viewModel);

            var result = controller.Edit(1) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Hambúrguer X", ((ItemcardapioViewModel)result.Model).Nome);
        }

        [TestMethod]
        public void EditPostTest_ModeloValido()
        {
            var model = GetTargetItemcardapioModel();
            var entidade = GetTargetItemcardapio();

            mockService.Setup(s => s.Get(1)).Returns(entidade);
            mockMapper.Setup(m => m.Map(model, entidade));

            var result = controller.Edit(1, model) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Edit(entidade), Times.Once);
        }

        [TestMethod]
        public void DeleteGetTest()
        {
            var itemcardapio = GetTargetItemcardapio();
            var viewModel = GetTargetItemcardapioModel();

            mockService.Setup(s => s.Get(1)).Returns(itemcardapio);
            mockMapper.Setup(m => m.Map<ItemcardapioViewModel>(itemcardapio)).Returns(viewModel);

            var result = controller.Delete(1) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteConfirmedTest()
        {
            mockService.Setup(s => s.Get(1)).Returns(GetTargetItemcardapio());

            var result = controller.DeleteConfirmed(1) as RedirectToActionResult;

            Assert.AreEqual("Index", result.ActionName);
            mockService.Verify(s => s.Delete(1), Times.Once);
        }

        #region Helpers

        private Itemcardapio GetTargetItemcardapio() =>
            new Itemcardapio
            {
                Id = 1,
                Nome = "Hambúrguer X",
                PrecoUnitario = 30f,
                Porcao = 1,
                DiaSemana = "Segunda",
                Status = true,
                Descricao = "Pão, carne, queijo, alface, tomate",
                Categoria = 1,
                Imagem = System.Text.Encoding.UTF8.GetBytes("placeholder")
            };

        private ItemcardapioViewModel GetTargetItemcardapioModel() =>
            new ItemcardapioViewModel
            {
                Id = 1,
                Nome = "Hambúrguer X",
                PrecoUnitario = 30f,
                Porcao = 1,
                DiasSemana = new List<string> { "Segunda" },
                Status = true,
                Descricao = "Pão, carne, queijo, alface, tomate",
                Categoria = "1"
            };

        private ItemcardapioViewModel GetCadastroCompletoModel() =>
            new ItemcardapioViewModel
            {
                Nome = "Hambúrguer X",
                PrecoUnitario = 30f,
                Porcao = 1,
                DiasSemana = new List<string> { "Segunda" },
                Status = true,
                Descricao = "Pão, carne, queijo, alface, tomate",
                Categoria = "1"
            };

        private IEnumerable<Itemcardapio> GetTestItemcardapios() =>
            new List<Itemcardapio>
            {
                new Itemcardapio 
                { 
                    Id = 1, 
                    Nome = "Hambúrguer X", 
                    PrecoUnitario = 30f, 
                    Status = true, 
                    DiaSemana = "Segunda", 
                    Categoria = 1, 
                    Porcao = 1, 
                    Imagem = System.Text.Encoding.UTF8.GetBytes("placeholder") 
                },
                new Itemcardapio 
                { 
                    Id = 2, 
                    Nome = "Pizza Grande", 
                    PrecoUnitario = 50f, 
                    Status = true, 
                    DiaSemana = "Terça", 
                    Categoria = 1, 
                    Porcao = 2, 
                    Imagem = System.Text.Encoding.UTF8.GetBytes("placeholder") 
                },
                new Itemcardapio 
                { 
                    Id = 3, 
                    Nome = "Fritas", 
                    PrecoUnitario = 15f, 
                    Status = true, 
                    DiaSemana = "Quarta", 
                    Categoria = 2, 
                    Porcao = 1, 
                    Imagem = System.Text.Encoding.UTF8.GetBytes("placeholder") 
                }
            };

        #endregion
    }
}
