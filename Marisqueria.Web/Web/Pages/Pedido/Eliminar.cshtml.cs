using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace Web.Pages.Pedido
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EliminarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public PedidoResponse pedido { get; set; } = new PedidoResponse();

        public async Task<IActionResult> OnGet(Guid id)
        {
            var encontrado = await CargarPedido(id);

            if (!encontrado)
            {
                return NotFound();
            }

            if (pedido.Estado != "Pendiente")
            {
                return RedirectToPage(
                    "./Detalle",
                    new { id });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarPedido");

            endpoint = string.Format(endpoint, id);

            var cliente = new HttpClient();

            var respuesta = await cliente.DeleteAsync(endpoint);

            if (!respuesta.IsSuccessStatusCode)
            {
                var mensaje = await respuesta.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty,mensaje);

                await CargarPedido(id);

                return Page();
            }
            
            return RedirectToPage("./Index");
        }

        private async Task<bool> CargarPedido(Guid id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPedido");

            endpoint = string.Format(endpoint, id);

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(endpoint);

            if (!respuesta.IsSuccessStatusCode)
            {
                return false;
            }

            pedido =
                await respuesta.Content.ReadFromJsonAsync<PedidoResponse>() ?? new PedidoResponse();

            return pedido.IdPedido != Guid.Empty;
        }
    }
}