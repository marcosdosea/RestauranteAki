using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestauranteAkiWeb.Areas.Identity.Data;
using RestauranteAkiWeb.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestauranteAkiWebTests.Controllers
{
    [TestClass()]
    public class LoginModelTests
    {
        private Mock<SignInManager<UsuarioIdentity>> _mockSignInManager;
        private Mock<UserManager<UsuarioIdentity>> _mockUserManager;
        private Mock<IUserStore<UsuarioIdentity>> _mockUserStore;
        private Mock<ILogger<LoginModel>> _mockLogger;
        private LoginModel _loginModel;
        private Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private DefaultHttpContext _httpContext;

        [TestInitialize]
        public void Setup()
        {
            // Mock UserStore
            _mockUserStore = new Mock<IUserStore<UsuarioIdentity>>();

            // Mock UserManager
            _mockUserManager = new Mock<UserManager<UsuarioIdentity>>(
                _mockUserStore.Object,
                null, null, null, null, null, null, null, null);

            // Mock HttpContextAccessor e HttpContext
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Scheme = "https";
            _httpContext.Request.Host = new HostString("localhost");
            _httpContext.Request.PathBase = "";

            // Configurar ServiceProvider com serviços de autenticação
            var serviceCollection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

            // Mock do IAuthenticationService
            var mockAuthService = new Mock<IAuthenticationService>();
            mockAuthService.Setup(x => x.SignOutAsync(
                It.IsAny<HttpContext>(),
                It.IsAny<string>(),
                It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            serviceCollection.AddSingleton(mockAuthService.Object);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _httpContext.RequestServices = serviceProvider;

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(_httpContext);

            // Mock SignInManager
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<UsuarioIdentity>>();
            _mockSignInManager = new Mock<SignInManager<UsuarioIdentity>>(
                _mockUserManager.Object,
                _mockHttpContextAccessor.Object,
                userPrincipalFactory.Object,
                null, null, null, null);

            // Mock Logger
            _mockLogger = new Mock<ILogger<LoginModel>>();

            // Criar instância do LoginModel
            _loginModel = new LoginModel(
                _mockSignInManager.Object,
                _mockLogger.Object);

            // Configurar PageContext
            var modelState = new ModelStateDictionary();
            var routeData = new RouteData();
            var actionContext = new ActionContext(_httpContext, routeData, new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext);
            _loginModel.PageContext = pageContext;

            // Mock do UrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper.Setup(x => x.Content(It.IsAny<string>()))
                .Returns((string s) => s);
            mockUrlHelper.Setup(x => x.ActionContext)
                .Returns(actionContext);

            _loginModel.Url = mockUrlHelper.Object;

            // Mock do TempData
            var tempDataProvider = new Mock<ITempDataProvider>();
            var tempDataDictionaryFactory = new TempDataDictionaryFactory(tempDataProvider.Object);
            _loginModel.TempData = tempDataDictionaryFactory.GetTempData(_httpContext);
        }

        [TestMethod()]
        public void LoginModelTest()
        {
            // Arrange & Act
            var model = new LoginModel(
                _mockSignInManager.Object,
                _mockLogger.Object);

            // Assert
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.PageContext);
        }

        [TestMethod()]
        public async Task OnGetAsyncTest_DeveCarregarExternalLogins()
        {
            // Arrange
            var externalLogins = new List<AuthenticationScheme>
            {
                new AuthenticationScheme("Google", "Google", typeof(IAuthenticationHandler)),
                new AuthenticationScheme("Facebook", "Facebook", typeof(IAuthenticationHandler))
            };

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(externalLogins);

            // Act
            await _loginModel.OnGetAsync("~/Home");

            // Assert
            Assert.AreEqual("~/Home", _loginModel.ReturnUrl);
            Assert.IsNotNull(_loginModel.ExternalLogins);
            Assert.AreEqual(2, _loginModel.ExternalLogins.Count);
        }

        [TestMethod()]
        public async Task OnGetAsyncTest_ComErrorMessage_DeveAdicionarErroAoModelState()
        {
            // Arrange
            _loginModel.ErrorMessage = "Erro de autenticação externa";

            var externalLogins = new List<AuthenticationScheme>();
            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(externalLogins);

            // Act
            await _loginModel.OnGetAsync();

            // Assert
            Assert.IsFalse(_loginModel.ModelState.IsValid);
            Assert.IsTrue(_loginModel.ModelState.Values.Any(v =>
                v.Errors.Any(e => e.ErrorMessage.Contains("Erro de autenticação externa"))));
        }

        [TestMethod()]
        public async Task OnGetAsyncTest_ReturnUrlNulo_DeveUsarPadraoTilSlash()
        {
            // Arrange
            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            await _loginModel.OnGetAsync(null);

            // Assert
            Assert.AreEqual("~/", _loginModel.ReturnUrl);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_ModelStateInvalido_DeveRetornarPage()
        {
            // Arrange
            _loginModel.ModelState.AddModelError("Email", "Email é obrigatório");

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_LoginComSucesso_DeveRedirecionarParaReturnUrl()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = false
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync("~/Dashboard");

            // Assert
            Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
            var redirectResult = result as LocalRedirectResult;
            Assert.AreEqual("~/Dashboard", redirectResult.Url);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_LoginComSucesso_ComRememberMe_DeveLogarComPersistencia()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = true
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                "teste@teste.com",
                "Senha123",
                true,
                false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
            _mockSignInManager.Verify(x => x.PasswordSignInAsync(
                "teste@teste.com",
                "Senha123",
                true,
                false), Times.Once);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_LoginFalhou_DeveAdicionarErroAoModelState()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "SenhaErrada",
                RememberMe = false
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(_loginModel.ModelState.IsValid);
            Assert.IsTrue(_loginModel.ModelState.Values.Any(v =>
                v.Errors.Any(e => e.ErrorMessage.Contains("Invalid login attempt"))));
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_ContaBloqueada_DeveRedirecionarParaLockout()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = false
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirectResult = result as RedirectToPageResult;
            Assert.AreEqual("./Lockout", redirectResult.PageName);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_RequerDoisFatores_DeveRedirecionarParaLoginWith2fa()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = true
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.TwoFactorRequired);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync("~/Dashboard");

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirectResult = result as RedirectToPageResult;
            Assert.AreEqual("./LoginWith2fa", redirectResult.PageName);

            // Verificar RouteValues
            Assert.IsNotNull(redirectResult.RouteValues);
            Assert.AreEqual("~/Dashboard", redirectResult.RouteValues["ReturnUrl"]);
            Assert.AreEqual(true, redirectResult.RouteValues["RememberMe"]);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_ReturnUrlNulo_DeveUsarPadraoTilSlash()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = false
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
            var redirectResult = result as LocalRedirectResult;
            Assert.AreEqual("~/", redirectResult.Url);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_LoginComSucesso_DeveLogarInformacao()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = false
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("User logged in")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_ContaBloqueada_DeveLogarWarning()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                RememberMe = false
            };

            _mockSignInManager.Setup(x => x.PasswordSignInAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.LockedOut);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            await _loginModel.OnPostAsync();

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("User account locked out")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_EmailVazio_DeveRetornarPage()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "",
                Password = "Senha123",
                RememberMe = false
            };

            _loginModel.ModelState.AddModelError("Email", "The Email field is required.");

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(_loginModel.ModelState.IsValid);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_SenhaVazia_DeveRetornarPage()
        {
            // Arrange
            _loginModel.Input = new LoginModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "",
                RememberMe = false
            };

            _loginModel.ModelState.AddModelError("Password", "The Password field is required.");

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _loginModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(_loginModel.ModelState.IsValid);
        }
    }
}