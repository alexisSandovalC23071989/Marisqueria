using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Security.Claims;

namespace Web.Pages.Pedido
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public IList<PedidoResponse> pedidos { get; set; } = new List<PedidoResponse>();

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGet()
        {
            var idUsuarioTexto = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(idUsuarioTexto))
            {
                pedidos = new List<PedidoResponse>();
                return;
            }

            if (!Guid.TryParse(idUsuarioTexto, out Guid idUsuario))
            {
                pedidos = new List<PedidoResponse>();
                return;
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPedidosPorUsuario");

            endpoint = string.Format(endpoint, idUsuario);

            Console.WriteLine($"ID USUARIO CLAIM: {idUsuario}");
            Console.WriteLine($"ENDPOINT FINAL: {endpoint}");

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                pedidos = new List<PedidoResponse>();
                return;
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            pedidos = JsonSerializer.Deserialize<List<PedidoResponse>>(
                resultado,
                opciones
            ) ?? new List<PedidoResponse>();
        }
    }
}