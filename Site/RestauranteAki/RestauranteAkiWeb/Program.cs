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

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("RestauranteAkiConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'RestauranteAkiConnection' não foi encontrada ou está vazia.");
}
builder.Services.AddDbContext<RestauranteAkiContext>(options => options.UseMySQL(connectionString));


builder.Services.AddDbContext<IdentityContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("IdentityContextConnection")));

builder.Services.AddDefaultIdentity<UsuarioIdentity>(options =>
{

    //SignIn settings.
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

    //Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;

    //default User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+!#$&*";
    options.User.RequireUniqueEmail = false;

    //default Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

}).AddEntityFrameworkStores<IdentityContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.Cookie.Name = "RestauranteAkiAuthCookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";

    //returnUrlParameter requires
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Login");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/Register");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/AccessDenied");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ForgotPassword");
    options.Conventions.AllowAnonymousToAreaPage("Identity", "/Account/ResetPassword");
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
