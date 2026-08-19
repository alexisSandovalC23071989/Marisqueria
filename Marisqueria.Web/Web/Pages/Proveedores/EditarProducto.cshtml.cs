using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Proveedores
{
    public class EditarProductoModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarProductoModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoProveedorRequest productoProveedor { get; set; } = new ProductoProveedorRequest();

        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; } = string.Empty;
        public string NombreProducto { get; set; } = string.Empty;

        public async Task<ActionResult> OnGet(int idProveedor, int idProducto)
        {
            IdProveedor = idProveedor;

            string endpointProveedor = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProveedor");
            var cliente = new HttpClient();
            var solicitudProveedor = new HttpRequestMessage(HttpMethod.Get, string.Format(endpointProveedor, idProveedor));
            var respuestaProveedor = await cliente.SendAsync(solicitudProveedor);
            if (respuestaProveedor.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }
            respuestaProveedor.EnsureSuccessStatusCode();

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var proveedor = JsonSerializer.Deserialize<ProveedorResponse>(await respuestaProveedor.Content.ReadAsStringAsync(), opciones);
            NombreProveedor = proveedor?.Empresa ?? string.Empty;

            string endpointProductos = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductosPorProveedor");
            var solicitudProductos = new HttpRequestMessage(HttpMethod.Get, string.Format(endpointProductos, idProveedor));
            var respuestaProductos = await cliente.SendAsync(solicitudProductos);
            respuestaProductos.EnsureSuccessStatusCode();

            var productos = JsonSerializer.Deserialize<List<ProductoProveedorResponse>>(await respuestaProductos.Content.ReadAsStringAsync(), opciones) ?? new List<ProductoProveedorResponse>();
            var productoActual = productos.FirstOrDefault(producto => producto.IdProducto == idProducto);

            if (productoActual == null)
            {
                return NotFound();
            }

            NombreProducto = productoActual.Producto;
            productoProveedor = new ProductoProveedorRequest
            {
                IdProducto = productoActual.IdProducto,
                PrecioCompra = productoActual.PrecioCompra,
                UnidadMedida = productoActual.UnidadMedida
            };

            return Page();
        }

        public async Task<ActionResult> OnPost(int idProveedor, int idProducto)
        {
            IdProveedor = idProveedor;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            productoProveedor.IdProducto = idProducto;

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProductoProveedor");
            var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);

            var respuesta = await cliente.PutAsJsonAsync(string.Format(endpoint, idProveedor, idProducto), productoProveedor);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Detalle", new { id = idProveedor });
        }
    }
}
