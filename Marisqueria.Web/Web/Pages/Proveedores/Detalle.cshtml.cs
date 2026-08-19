using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Proveedores
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public ProveedorResponse proveedor { get; set; } = new ProveedorResponse();
        public List<ProductoProveedorResponse> productos { get; set; } = new List<ProductoProveedorResponse>();

        public async Task<ActionResult> OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            if (!await CargarProveedor(id.Value))
            {
                return NotFound();
            }

            await CargarProductos(id.Value);

            return Page();
        }

        public async Task<ActionResult> OnPostEliminarProductoAsync(int idProveedor, int idProducto)
        {
            if (!User.IsInRole("Administrador")) return Forbid();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarProductoProveedor");
            var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);

            var respuesta = await cliente.DeleteAsync(string.Format(endpoint, idProveedor, idProducto));
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Detalle", new { id = idProveedor });
        }

        private async Task<bool> CargarProveedor(int id)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProveedor");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);
            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var proveedorEncontrado = JsonSerializer.Deserialize<ProveedorResponse>(resultado, opciones);

            if (proveedorEncontrado == null)
            {
                return false;
            }

            proveedor = proveedorEncontrado;
            return true;
        }

        private async Task CargarProductos(int idProveedor)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductosPorProveedor");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idProveedor));

            var respuesta = await cliente.SendAsync(solicitud);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                productos = new List<ProductoProveedorResponse>();
                return;
            }
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            productos = JsonSerializer.Deserialize<List<ProductoProveedorResponse>>(resultado, opciones) ?? new List<ProductoProveedorResponse>();
        }
    }
}
