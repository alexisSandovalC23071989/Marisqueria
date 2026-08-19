using Abstracciones.Interfaces.Reglas;
using Reglas;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

const string SoloAdministradores = "SoloAdministradores";

builder.Services.AddRazorPages(options =>
{
    string[] paginasAdministrativas =
    {
        "/Productos/Agregar",
        "/Productos/Editar",
        "/Productos/Eliminar",
        "/Categorias/Agregar",
        "/Categorias/Editar",
        "/Inventarios/Agregar",
        "/Inventarios/Editar",
        "/Proveedores/Agregar",
        "/Proveedores/Editar",
        "/Proveedores/AgregarProducto",
        "/Proveedores/EditarProducto",
        "/Promociones/Agregar",
        "/Promociones/Editar",
        "/Promociones/Desactivar",
        "/Promociones/Alertas"
    };

    foreach (var pagina in paginasAdministrativas)
    {
        options.Conventions.AuthorizePage(pagina, SoloAdministradores);
    }
});

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme)  //agregamos la autenticación por cookies
                                                        //CookieAuthenticationDefaults es una clase que contiene constantes relacionadas con la autenticación basada en cookies, como el esquema de autenticación predeterminado.         
    .AddCookie(options =>
    {
        options.LoginPath = "/Autenticacion/Login";  //ruta a la página de inicio de sesión
        options.AccessDeniedPath = "/Autenticacion/AccesoDenegado";  //ruta a la página de acceso denegado
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        SoloAdministradores,
        policy => policy.RequireRole("Administrador"));
});

builder.Services.AddScoped<IConfiguracion, Configuracion>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
