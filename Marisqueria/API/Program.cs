using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Abstracciones.Interfaces.Servicios;
using Flujo.Servicios;
using System.Text;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using DA;
using Flujo;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Se agrega la validacion del Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)//activa la autenticacion con JWT
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters //Parametros de validacion del token
        {
            ValidateIssuer = true,//valida el emisor del token
            ValidateAudience = true,//valida el destinatario del token
            ValidateLifetime = true,//valida la vida del token
            ValidateIssuerSigningKey = true,//valida la clave de firma del emisor

            ValidIssuer = builder.Configuration["Jwt:Issuer"],//valida el emisor del token
            ValidAudience = builder.Configuration["Jwt:Audience"],//valida el destinatario del token

            IssuerSigningKey = new SymmetricSecurityKey(//valida la clave de firma del emisor
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))//convierte la clave de firma del emisor en bytes
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT generado al iniciar sesión."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<DBContext>();
builder.Services.AddScoped<IProductosFlujo, ProductosFlujo>();
builder.Services.AddScoped<IProductosDA, ProductosDA>();
builder.Services.AddScoped<ICategoriasFlujo, CategoriasFlujo>();
builder.Services.AddScoped<ICategoriasDA, CategoriasDA>();
builder.Services.AddScoped<IInventariosFlujo, InventariosFlujo>();
builder.Services.AddScoped<IInventariosDA, InventariosDA>();
builder.Services.AddScoped<IUsuarioFlujo, UsuarioFlujo>();
builder.Services.AddScoped<IUsuarioDA, UsuarioDA>();
builder.Services.AddScoped<IProveedoresFlujo, ProveedoresFlujo>();
builder.Services.AddScoped<IProveedoresDA, ProveedoresDA>();
builder.Services.AddScoped<IPromocionesFlujo, PromocionesFlujo>();
builder.Services.AddScoped<IPromocionesDA, PromocionesDA>();
builder.Services.AddScoped<IResenasFlujo, ResenasFlujo>();
builder.Services.AddScoped<IResenasDA, ResenasDA>();
builder.Services.AddScoped<IPedidoFlujo, PedidoFlujo>();
builder.Services.AddScoped<IPedidoDA, PedidoDA>();
builder.Services.AddScoped<IDetallePedidoFlujo, DetallePedidoFlujo>();
builder.Services.AddScoped<IDetallePedidoDA, DetallePedidoDA>();
builder.Services.AddScoped<ICorreoServicio, CorreoServicio>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
