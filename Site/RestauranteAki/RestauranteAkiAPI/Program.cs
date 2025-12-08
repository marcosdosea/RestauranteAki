using Core;
using Core.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestauranteAkiWeb.Areas.Identity.Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IItemcardapioService, ItemcardapioService>();  //rotas

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var connectionString = builder.Configuration.GetConnectionString("RestauranteAkiConnection");
var connectionStringIdentity = builder.Configuration.GetConnectionString("IdentityContextConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'RestauranteAkiConnection' não foi encontrada ou está vazia.");
}
builder.Services.AddDbContext<RestauranteAkiContext>(options => options.UseMySQL(connectionString));
builder.Services.AddDbContext<IdentityContext>(options => options.UseMySQL(connectionStringIdentity));

//Configure tokens life
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
    //sets a 2 hour lifetime of the generated token to reset password/email/phone number
    options.TokenLifespan = TimeSpan.FromHours(2)
);

builder.Services.ConfigureApplicationCookie(options =>
{
    //options.AccessDeniedPath = "/Identity/Autenticar";
    options.Cookie.Name = "BibliotecaCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    //options.LoginPath = "/Identity/Autenticar";
    // ReturnUrlParameter requires 
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
