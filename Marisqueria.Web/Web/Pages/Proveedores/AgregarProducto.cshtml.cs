using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Proveedores
{
    public class AgregarProductoModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public AgregarProductoModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoProveedorRequest productoProveedor { get; set; } = new ProductoProveedorRequest();

        [BindProperty]
        public List<SelectListItem> productos { get; set; } = new List<SelectListItem>();

        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; } = string.Empty;

        public async Task<ActionResult> OnGet(int idProveedor)
        {
            IdProveedor = idProveedor;

            if (!await CargarNombreProveedor(idProveedor))
            {
                return NotFound();
            }

            await ObtenerProductos();
            return Page();
        }

        public async Task<ActionResult> OnPost(int idProveedor)
        {
            IdProveedor = idProveedor;

            if (!ModelState.IsValid)
            {
                await CargarNombreProveedor(idProveedor);
                await ObtenerProductos();
                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProductoProveedor");
            var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);

            var respuesta = await cliente.PostAsJsonAsync(string.Format(endpoint, idProveedor), productoProveedor);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Detalle", new { id = idProveedor });
        }

        private async Task<bool> CargarNombreProveedor(int idProveedor)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProveedor");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, idProveedor));

            var respuesta = await cliente.SendAsync(solicitud);
            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var proveedor = JsonSerializer.Deserialize<ProveedorResponse>(resultado, opciones);

            NombreProveedor = proveedor?.Empresa ?? string.Empty;
            return proveedor != null;
        }

        private async Task ObtenerProductos()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                productos = new List<SelectListItem>();
                return;
            }
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadoSerializado = JsonSerializer.Deserialize<List<ProductoResponse>>(resultado, opciones) ?? new List<ProductoResponse>();

            productos = resultadoSerializado.Select(producto =>
                new SelectListItem
                {
                    Value = producto.IdProducto.ToString(),
                    Text = producto.Nombre
                }
            ).ToList();
        }
    }
}
