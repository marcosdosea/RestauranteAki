using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Registra AutoMapper e procura automaticamente os profiles
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Seus serviços
builder.Services.AddTransient<IPessoaService, PessoaService>();
builder.Services.AddTransient<IPedidoService, PedidoService>();
builder.Services.AddTransient<IGarcomService, GarcomService>();
builder.Services.AddTransient<IItemcardapioService, ItemcardapioService>();
builder.Services.AddTransient<IRestauranteService, RestauranteService>();

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("RestauranteAkiConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("A string de conexão 'RestauranteAkiConnection' não foi encontrada ou está vazia.");
}
builder.Services.AddDbContext<RestauranteAkiContext>(options => options.UseMySQL(connectionString));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
