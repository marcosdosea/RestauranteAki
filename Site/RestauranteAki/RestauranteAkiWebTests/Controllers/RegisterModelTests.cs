using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestauranteAkiWeb.Areas.Identity.Data;
using RestauranteAkiWeb.Areas.Identity.Pages.Account;
using System.Security.Claims;
using System.Threading;

namespace RestauranteAkiWebTests.Controllers
{
    [TestClass()]
    public class RegisterModelTests
    {
        private Mock<UserManager<UsuarioIdentity>> _mockUserManager;
        private Mock<SignInManager<UsuarioIdentity>> _mockSignInManager;
        private Mock<IUserStore<UsuarioIdentity>> _mockUserStore;
        private Mock<IUserEmailStore<UsuarioIdentity>> _mockEmailStore;
        private Mock<ILogger<RegisterModel>> _mockLogger;
        private Mock<IEmailSender> _mockEmailSender;
        private RegisterModel _registerModel;
        private IdentityOptions _identityOptions;

        [TestInitialize]
        public void Setup()
        {
            // Mock UserStore
            _mockUserStore = new Mock<IUserStore<UsuarioIdentity>>();
            _mockEmailStore = _mockUserStore.As<IUserEmailStore<UsuarioIdentity>>();

            // Criar IdentityOptions
            _identityOptions = new IdentityOptions
            {
                SignIn = { RequireConfirmedAccount = false },
                Password =
        {
            RequireDigit = true,
            RequireLowercase = false,
            RequireNonAlphanumeric = false,
            RequireUppercase = false,
            RequiredLength = 6
        }
            };

            // Mock UserManager com Options
            var optionsAccessor = new Mock<Microsoft.Extensions.Options.IOptions<IdentityOptions>>();
            optionsAccessor.Setup(x => x.Value).Returns(_identityOptions);

            _mockUserManager = new Mock<UserManager<UsuarioIdentity>>(
                _mockUserStore.Object,
                optionsAccessor.Object,
                null, null, null, null, null, null, null);

            // Mock SignInManager
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipalFactory = new Mock<IUserClaimsPrincipalFactory<UsuarioIdentity>>();
            _mockSignInManager = new Mock<SignInManager<UsuarioIdentity>>(
                _mockUserManager.Object,
                contextAccessor.Object,
                userPrincipalFactory.Object,
                null, null, null, null);

            // Mock Logger e EmailSender
            _mockLogger = new Mock<ILogger<RegisterModel>>();
            _mockEmailSender = new Mock<IEmailSender>();

            // Configurar UserManager para suportar email
            _mockUserManager.Setup(x => x.SupportsUserEmail).Returns(true);

            // Criar instância do RegisterModel
            _registerModel = new RegisterModel(
                _mockUserManager.Object,
                _mockUserStore.Object,
                _mockSignInManager.Object,
                _mockLogger.Object,
                _mockEmailSender.Object);

            // Configurar contexto HTTP com Request completo
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost");
            httpContext.Request.PathBase = "";

            var modelState = new ModelStateDictionary();
            var routeData = new RouteData();

            var actionContext = new ActionContext(httpContext, routeData, new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext);
            _registerModel.PageContext = pageContext;

            // Criar mock do UrlHelper com suporte completo
            var mockUrlHelper = new Mock<IUrlHelper>();

            // Mock para Content
            mockUrlHelper.Setup(x => x.Content(It.IsAny<string>()))
                .Returns((string s) => s);

            // Mock para Action
            mockUrlHelper.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns((UrlActionContext context) =>
                {
                    var query = "";
                    if (context.Values != null)
                    {
                        var values = new RouteValueDictionary(context.Values);
                        var queryParams = string.Join("&", values.Select(kv => $"{kv.Key}={kv.Value}"));
                        query = "?" + queryParams;
                    }
                    return $"https://localhost{context.Action}{query}";
                });

            // Mock para RouteUrl (essencial para Url.Page())
            mockUrlHelper.Setup(x => x.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns((UrlRouteContext context) =>
                {
                    return "https://localhost/Account/ConfirmEmail?userId=123&code=abc&returnUrl=%2F";
                });

            // IMPORTANTE: Mock para ActionContext (necessário para Url.Page())
            mockUrlHelper.Setup(x => x.ActionContext)
                .Returns(actionContext);

            _registerModel.Url = mockUrlHelper.Object;
        }

        [TestMethod()]
        public void RegisterModelTest()
        {
            // Arrange & Act
            var model = new RegisterModel(
                _mockUserManager.Object,
                _mockUserStore.Object,
                _mockSignInManager.Object,
                _mockLogger.Object,
                _mockEmailSender.Object);

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
            await _registerModel.OnGetAsync("~/Home");

            // Assert
            Assert.AreEqual("~/Home", _registerModel.ReturnUrl);
            Assert.IsNotNull(_registerModel.ExternalLogins);
            Assert.AreEqual(2, _registerModel.ExternalLogins.Count);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_ModelStateInvalido_DeveRetornarPage()
        {
            // Arrange
            _registerModel.ModelState.AddModelError("Email", "Email é obrigatório");

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_RegistroComSucesso_DeveCriarUsuarioEEnviarEmail()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                ConfirmPassword = "Senha123"
            };

            var usuario = new UsuarioIdentity { Email = "teste@teste.com", UserName = "teste@teste.com" };

            _mockEmailStore.Setup(x => x.SetEmailAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserStore.Setup(x => x.SetUserNameAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<UsuarioIdentity>()))
                .ReturnsAsync("user-id-123");

            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<UsuarioIdentity>()))
                .ReturnsAsync("confirmation-token");

            _mockSignInManager.Setup(x => x.SignInAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<bool>(), null))
                .Returns(Task.CompletedTask);

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _registerModel.OnPostAsync("~/");

            // Assert
            Assert.IsInstanceOfType(result, typeof(LocalRedirectResult));
            _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<UsuarioIdentity>(), "Senha123"), Times.Once);
            _mockEmailSender.Verify(x => x.SendEmailAsync(
                "teste@teste.com",
                "Confirm your email",
                It.IsAny<string>()), Times.Once);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_RegistroComSucesso_ContaRequerConfirmacao_DeveRedirecionarParaConfirmacao()
        {
            // Arrange
            // Alterar a opção para requerer confirmação de conta
            _identityOptions.SignIn.RequireConfirmedAccount = true;

            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "Senha123",
                ConfirmPassword = "Senha123"
            };

            _mockEmailStore.Setup(x => x.SetEmailAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserStore.Setup(x => x.SetUserNameAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockUserManager.Setup(x => x.GetUserIdAsync(It.IsAny<UsuarioIdentity>()))
                .ReturnsAsync("user-id-123");

            _mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(It.IsAny<UsuarioIdentity>()))
                .ReturnsAsync("confirmation-token");

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToPageResult));
            var redirectResult = result as RedirectToPageResult;
            Assert.AreEqual("RegisterConfirmation", redirectResult.PageName);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_FalhaAoCriarUsuario_DeveAdicionarErrosAoModelState()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "123",
                ConfirmPassword = "123"
            };

            var errors = new[]
            {
                new IdentityError { Code = "PasswordTooShort", Description = "A senha deve ter pelo menos 6 caracteres." },
                new IdentityError { Code = "PasswordRequiresDigit", Description = "A senha deve conter ao menos um dígito." }
            };

            _mockEmailStore.Setup(x => x.SetEmailAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserStore.Setup(x => x.SetUserNameAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(_registerModel.ModelState.IsValid);
            Assert.AreEqual(2, _registerModel.ModelState.ErrorCount);
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_EmailDuplicado_DeveRetornarErro()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "duplicado@teste.com",
                Password = "Senha123",
                ConfirmPassword = "Senha123"
            };

            var errors = new[]
            {
                new IdentityError { Code = "DuplicateEmail", Description = "O email 'duplicado@teste.com' já está sendo usado." }
            };

            _mockEmailStore.Setup(x => x.SetEmailAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserStore.Setup(x => x.SetUserNameAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(_registerModel.ModelState.IsValid);
            Assert.IsTrue(_registerModel.ModelState.Values.Any(v =>
                v.Errors.Any(e => e.ErrorMessage.Contains("já está sendo usado"))));
        }

        [TestMethod()]
        public async Task OnPostAsyncTest_SenhaInvalida_DeveRetornarErro()
        {
            // Arrange
            _registerModel.Input = new RegisterModel.InputModel
            {
                Email = "teste@teste.com",
                Password = "abc", // Senha muito curta
                ConfirmPassword = "abc"
            };

            var errors = new[]
            {
                new IdentityError { Code = "PasswordTooShort", Description = "A senha deve ter pelo menos 6 caracteres." },
                new IdentityError { Code = "PasswordRequiresDigit", Description = "A senha deve conter ao menos um dígito." }
            };

            _mockEmailStore.Setup(x => x.SetEmailAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserStore.Setup(x => x.SetUserNameAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<UsuarioIdentity>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(errors));

            _mockSignInManager.Setup(x => x.GetExternalAuthenticationSchemesAsync())
                .ReturnsAsync(new List<AuthenticationScheme>());

            // Act
            var result = await _registerModel.OnPostAsync();

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageResult));
            Assert.IsFalse(_registerModel.ModelState.IsValid);
        }
    }
}