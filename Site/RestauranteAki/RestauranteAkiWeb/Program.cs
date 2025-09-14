using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using RestauranteAkiWeb.Areas.Identity.Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages(options =>
{
    // (Opcional, mas ajuda a blindar contra loop)
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPassword");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ResetPassword");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/AccessDenied");
});

// Registra AutoMapper e procura automaticamente os profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Seus serviços
builder.Services.AddTransient<IPessoaService, PessoaService>();
builder.Services.AddTransient<IPedidoService, PedidoService>();
builder.Services.AddTransient<IGarcomService, GarcomService>();
builder.Services.AddTransient<IItemcardapioService, ItemcardapioService>();
builder.Services.AddTransient<ICardapioService, CardapioService>();
builder.Services.AddTransient<IMesaService, MesaService>();
builder.Services.AddTransient<IRestauranteService, RestauranteService>();
builder.Services.AddTransient<IContumService, ContumService>();
builder.Services.AddTransient<IPersonagemService, PersonagemService>();

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("RestauranteAkiConnection");
var connectionStringIdentity = builder.Configuration.GetConnectionString("IdentityContextConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'RestauranteAkiConnection' não foi encontrada ou está vazia.");
}
builder.Services.AddDbContext<RestauranteAkiContext>(options => options.UseMySQL(connectionString));
builder.Services.AddDbContext<IdentityContext>(options => options.UseMySQL(connectionStringIdentity));

builder.Services.AddDefaultIdentity<UsuarioIdentity>(options => {
    //SingIn Settings
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    //Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;

    //default user settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;

    //default lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

}).AddEntityFrameworkStores<IdentityContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.LogoutPath = "/Identity/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.Cookie.Name = "RestauranteAkiCookie";
    options.Cookie.HttpOnly = true;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;

});

builder.Services.AddHttpClient();
builder.Services.AddScoped<ViaCepService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
