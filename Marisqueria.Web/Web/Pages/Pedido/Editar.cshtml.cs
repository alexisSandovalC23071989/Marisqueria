using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Pages.Pedido
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public Guid IdPedido { get; set; }

        public PedidoResponse pedido { get; set; } = new PedidoResponse();

        public IList<DetallePedidoResponse> detalles { get; set; } = new List<DetallePedidoResponse>();

        [BindProperty]
        public int IdCategoria { get; set; }

        [BindProperty]
        public int IdProducto { get; set; }

        [BindProperty]
        public int CantidadNueva { get; set; } = 1;

        public List<SelectListItem> categorias { get; set; } = new List<SelectListItem>();

        public List<ProductoResponse> productos { get; set; } = new List<ProductoResponse>();

        public Dictionary<int, decimal> stockProductos { get; set; } = new Dictionary<int, decimal>();

        public async Task<IActionResult> OnGet(Guid id)
        {
            IdPedido = id;

            await CargarPedido();
            if (pedido.Estado != "Pendiente")
            {
                return RedirectToPage("./Detalle", new { id = IdPedido });
            }
            await CargarDetalles();
            await CargarCategorias();
            await CargarProductos();
            await CargarStockProductos();

            return Page();
        }

        private async Task RecargarPagina()
        {
            await CargarPedido();
            await CargarDetalles();
            await CargarCategorias();
            await CargarProductos();
            await CargarStockProductos();
        }

        public async Task<IActionResult> OnPostAgregarProducto()
        {
            if (IdProducto <= 0)
            {
                ModelState.AddModelError(string.Empty, "Debe seleccionar un producto.");

                await RecargarPagina();
                return Page();
            }

            if (CantidadNueva <= 0)
            {
                ModelState.AddModelError(string.Empty, "La cantidad debe ser mayor a cero.");

                await RecargarPagina();
                return Page();
            }

            string endpointStock = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerStockProducto");

            endpointStock = string.Format(endpointStock, IdProducto);

            var clienteStock = new HttpClient();

            var respuestaStock = await clienteStock.GetAsync(endpointStock);

            if (!respuestaStock.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo consultar el stock del producto.");

                await RecargarPagina();

                return Page();
            }

            var stock = await respuestaStock.Content.ReadFromJsonAsync<StockProductoResponse>();

            if (stock == null || stock.StockDisponible < CantidadNueva)
            {
                ModelState.AddModelError(string.Empty, $"Stock insuficiente. Disponible: {stock?.StockDisponible ?? 0}.");

                await RecargarPagina();

                return Page();
            }

            string endpointProducto = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");

            endpointProducto = string.Format(endpointProducto, IdProducto);

            var cliente = new HttpClient();

            var respuestaProducto = await cliente.GetAsync(endpointProducto);

            respuestaProducto.EnsureSuccessStatusCode();

            var producto = await respuestaProducto.Content.ReadFromJsonAsync<ProductoResponse>();

            if (producto == null)
            {
                ModelState.AddModelError(string.Empty, "El producto no existe.");

                await RecargarPagina();
                return Page();
            }

            decimal precioUnitario = producto.PrecioVentaActual;

            decimal subtotal = precioUnitario * CantidadNueva;

            var detalle = new DetallePedidoRequest
            {
                IdPedido = IdPedido,
                IdProducto = IdProducto,
                Cantidad = CantidadNueva,
                PrecioUnitario = precioUnitario,
                Subtotal = subtotal
            };

            string endpointDetalle = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarDetallePedido");

            var respuesta = await cliente.PostAsJsonAsync(endpointDetalle, detalle);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Editar", new { id = IdPedido });
        }

        public async Task<IActionResult> OnPostActualizar(Guid idDetalle, int cantidad)
        {

            if (cantidad <= 0)
            {
                ModelState.AddModelError(string.Empty, "La cantidad debe ser mayor a cero.");

                await RecargarPagina();

                return Page();
            }

            string endpointObtener =_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerDetallePedido");

            endpointObtener = string.Format(endpointObtener, idDetalle);

            var cliente = new HttpClient();

            var respuestaDetalle = await cliente.GetAsync(endpointObtener);

            respuestaDetalle.EnsureSuccessStatusCode();

            var detalleActual =  await respuestaDetalle.Content.ReadFromJsonAsync<DetallePedidoResponse>();

            if (detalleActual == null)
            {
                return NotFound();
            }

            decimal subtotal = detalleActual.PrecioUnitario * cantidad;

            var detalleEditar = new DetallePedidoRequest
            {
                IdPedido = detalleActual.IdPedido,
                IdProducto = detalleActual.IdProducto,
                Cantidad = cantidad,
                PrecioUnitario = detalleActual.PrecioUnitario,
                Subtotal = subtotal
            };

            string endpointEditar = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarDetallePedido");

            endpointEditar = string.Format(endpointEditar, idDetalle);

            var respuesta = await cliente.PutAsJsonAsync(endpointEditar, detalleEditar);

            if (!respuesta.IsSuccessStatusCode)
            {
                var error = await respuesta.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    error);

                IdPedido = detalleActual.IdPedido;

                await RecargarPagina();

                return Page();
            }

            return RedirectToPage(
                "./Editar",
                new { id = detalleActual.IdPedido });
        }

        public async Task<IActionResult> OnPostEliminarProducto(Guid idDetalle)
        {
            string endpointObtener = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerDetallePedido");

            endpointObtener = string.Format(endpointObtener, idDetalle);

            var cliente = new HttpClient();

            var respuestaDetalle = await cliente.GetAsync(endpointObtener);

            respuestaDetalle.EnsureSuccessStatusCode();

            var detalleActual = await respuestaDetalle.Content.ReadFromJsonAsync<DetallePedidoResponse>();

            if (detalleActual == null)
                return NotFound();

            string endpointEliminar = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarDetallePedido");

            endpointEliminar = string.Format(endpointEliminar, idDetalle);

            var respuesta = await cliente.DeleteAsync(endpointEliminar);

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Editar", new { id = detalleActual.IdPedido });
        }

        private async Task CargarPedido()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPedido");

            endpoint = string.Format(endpoint, IdPedido);

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(endpoint);

            respuesta.EnsureSuccessStatusCode();

            pedido = await respuesta.Content.ReadFromJsonAsync<PedidoResponse>() ?? new PedidoResponse();
        }

        private async Task CargarDetalles()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerDetallesPorPedido");

            endpoint = string.Format(endpoint, IdPedido);

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(endpoint);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                detalles = new List<DetallePedidoResponse>();

                return;
            }

            respuesta.EnsureSuccessStatusCode();

            detalles = await respuesta.Content.ReadFromJsonAsync<List<DetallePedidoResponse>>() ?? new List<DetallePedidoResponse>();
        }

        private async Task CargarCategorias()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(endpoint);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                categorias = new List<SelectListItem>();
                return;
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var lista = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones) ?? new List<Categoria>();

            categorias =
                lista.Select(c => new SelectListItem
                {
                    Value = c.IdCategoria.ToString(),
                    Text = c.Nombre
                }).ToList();
        }

        private async Task CargarProductos()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos");

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(endpoint);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                productos = new List<ProductoResponse>();
                return;
            }

            productos = await respuesta.Content.ReadFromJsonAsync<List<ProductoResponse>>() ?? new List<ProductoResponse>();
        }

        private async Task CargarStockProductos()
        {
            stockProductos = new Dictionary<int, decimal>();

            foreach (var producto in productos)
            {
                string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerStockProducto");

                endpoint = string.Format(endpoint, producto.IdProducto);

                var cliente = new HttpClient();

                var respuesta = await cliente.GetAsync(endpoint);

                if (!respuesta.IsSuccessStatusCode)
                {
                    stockProductos[producto.IdProducto] = 0;
                    continue;
                }

                var stock = await respuesta.Content.ReadFromJsonAsync<StockProductoResponse>();

                stockProductos[producto.IdProducto] = stock?.StockDisponible ?? 0;
            }
        }
    }
}