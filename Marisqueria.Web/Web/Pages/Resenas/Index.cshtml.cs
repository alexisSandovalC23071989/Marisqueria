using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloResenas;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Resenas
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public IList<ResenaResponse> resenas { get; set; } = new List<ResenaResponse>();

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet(int idProducto = 0)
        {
            if (idProducto <= 0)
            {
                resenas = new List<ResenaResponse>();
                return;
            }

            string endpoint = string.Format(
                _configuracion.ObtenerMetodo(
                    "ApiEndPoints",
                    "ObtenerResenas"),
                idProducto);

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(
                HttpMethod.Get,
                endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                resenas = new List<ResenaResponse>();
                return;
            }

            var resultado =
                await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            resenas =
                JsonSerializer.Deserialize<List<ResenaResponse>>(
                    resultado,
                    opciones)
                ?? new List<ResenaResponse>();
        }
    }
}
