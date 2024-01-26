using ADS.Models;
using ADS.Services;
using Microsoft.EntityFrameworkCore;
using ADS.Services.ContratoSesion;
using ADS.Services.ImplementacionSesion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Constructor para usar servicios destinados al manejo de sesiones dentro de la bd
builder.Services.AddDbContext<BaseObrasContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Azure_DB"));
});

//Constructor para servicios de manejo de sesion en validacion
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

//Constructor del inicio de sesion y manejo de cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>{
        options.LoginPath = "/Inicio/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.AccessDeniedPath = "/Home/AccesoDenegado";
});



builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioProyectos, RepositorioProyectos>();
builder.Services.AddTransient<IRepositorioFrenteObra, RepositorioFrenteObra>();
builder.Services.AddTransient<IRepositorioReporteDiario, repositorioReporteDiario>();
builder.Services.AddTransient<IRepositorioFacturas, RepositorioFacturas>();
builder.Services.AddTransient<IRepositorioCatalogoConceptos, RepositorioCatalogoConceptos>();
builder.Services.AddTransient<IRepositorioConstructoras, RepositorioConstructoras>();

builder.Services.AddControllersWithViews(options => 
{ options.Filters.Add(
    new ResponseCacheAttribute
    {
        NoStore=true,
        Location = ResponseCacheLocation.None,
    }); 
});


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
//AUTENTICACION para manejo de sesiones
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=Login}/{id?}");
//Modifico inicio para que siempre muestre el login al inicOwo
app.Run();
