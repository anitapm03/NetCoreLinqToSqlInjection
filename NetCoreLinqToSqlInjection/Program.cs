using NetCoreLinqToSqlInjection.Models;
using NetCoreLinqToSqlInjection.Repositories;
using NetCoreLinqToSqlInjection.Repositories.RepositoriesOracle;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IRepositoryDoctores, RepositoryDoctoresSQLServer>();
//builder.Services.AddTransient<IRepositoryDoctores, RepositoryDoctoresOracle>();

//builder.Services.AddTransient<Coche>();
//builder.Services.AddSingleton<Coche>();
//builder.Services.AddSingleton<ICoche,Deportivo>();
Coche car = new Coche();
car.Marca = "Mini";
car.Modelo = "Cooper";
car.Imagen = "coche.jpg";
car.Velocidad = 0;
car.VelocidadMaxima = 300;
//para poder enviar el obj se usa lambda
builder.Services.AddSingleton<ICoche, Coche>(x => car);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
