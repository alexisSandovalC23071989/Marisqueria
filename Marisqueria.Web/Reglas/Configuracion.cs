using Abstracciones.Interfaces.Reglas;
using Microsoft.Extensions.Configuration;

namespace Reglas
{
    public class Configuracion : IConfiguracion
    {
        private readonly IConfiguration _configuration;

        public Configuracion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ObtenerMetodo(string seccion, string nombre)
        {
            string urlBase = ObtenerUrlBase(seccion);
            string metodo = _configuration.GetSection($"{seccion}:Metodos")
                .GetChildren()
                .FirstOrDefault(metodo => metodo["Nombre"] == nombre)?["Valor"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(metodo))
            {
                throw new InvalidOperationException($"No se encontro el metodo {nombre} en la seccion {seccion}.");
            }

            return $"{urlBase.TrimEnd('/')}/{metodo.TrimStart('/')}";
        }

        private string ObtenerUrlBase(string seccion)
        {
            string? urlBase = _configuration[$"{seccion}:UrlBase"];
            if (string.IsNullOrWhiteSpace(urlBase))
            {
                throw new InvalidOperationException($"No se encontro la UrlBase en la seccion {seccion}.");
            }

            return urlBase;
        }
    }
}

