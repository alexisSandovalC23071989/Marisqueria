using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace Web.Pages.Pedido
{
    [Authorize(Roles = "Administrador")]
    public class GestionModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public GestionModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public IList<PedidoResponse> pedidos { get; set; } = new List<PedidoResponse>();

        public async Task OnGet()
        {
            await ObtenerPedidos();
        }

        public async Task<IActionResult> OnPostConfirmar(Guid idPedido)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ConfirmarPedido");

            endpoint = string.Format(endpoint, idPedido);

            var cliente = new HttpClient();

            var respuesta = await cliente.PutAsync(endpoint, null);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Gestion");
        }

        public async Task<IActionResult> OnPostCancelar(Guid idPedido)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "CancelarPedido");

            endpoint = string.Format(endpoint, idPedido);

            var cliente = new HttpClient();

            var respuesta = await cliente.PutAsync(endpoint, null);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Gestion");
        }

        public async Task<IActionResult> OnPostPreparar(Guid idPedido)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "PrepararPedido");

            endpoint = string.Format(endpoint, idPedido);

            var cliente = new HttpClient();

            var respuesta = await cliente.PutAsync(endpoint, null);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Gestion");
        }

        public async Task<IActionResult> OnPostEntregar(Guid idPedido)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints","EntregarPedido");

            endpoint = string.Format(endpoint, idPedido);

            var cliente = new HttpClient();

            var respuesta = await cliente.PutAsync(endpoint, null);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Gestion");
        }

        private async Task ObtenerPedidos()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPedidos");

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                pedidos = new List<PedidoResponse>();
                return;
            }

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

            pedidos =
                JsonSerializer.Deserialize<List<PedidoResponse>>(resultado, opciones) ?? new List<PedidoResponse>();
        }
    }
}