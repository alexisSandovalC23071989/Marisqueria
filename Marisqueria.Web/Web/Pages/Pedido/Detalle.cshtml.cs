using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;
using System.Security.Claims;

namespace Web.Pages.Pedido
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public PedidoResponse pedido { get; set; } = new PedidoResponse();

        public string NombreUsuario { get; set; } = string.Empty;

        public IList<DetallePedidoResponse> detalles { get; set; }= new List<DetallePedidoResponse>();

        public async Task<IActionResult> OnGet(Guid id)
        {
            await ObtenerPedido(id);
            await ObtenerDetalles(id);

            NombreUsuario = User.Identity?.Name ?? "Usuario";

            return Page();
        }

        private async Task ObtenerPedido(Guid idPedido)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints","ObtenerPedido");

            endpoint = string.Format(endpoint, idPedido);

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get,endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            pedido = JsonSerializer.Deserialize<PedidoResponse>(resultado,opciones) ?? new PedidoResponse();
        }

        private async Task ObtenerDetalles(Guid idPedido)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints","ObtenerDetallesPorPedido");

            endpoint = string.Format(endpoint, idPedido);

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get,endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                detalles = new List<DetallePedidoResponse>();
                return;
            }

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            detalles =
                JsonSerializer.Deserialize<List<DetallePedidoResponse>>(resultado,opciones) ?? new List<DetallePedidoResponse>();
        }
    }
}