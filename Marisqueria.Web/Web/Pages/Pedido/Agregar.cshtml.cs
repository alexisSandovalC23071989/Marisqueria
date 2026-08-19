using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Abstracciones.Modelos.ModeloPedido;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Security.Claims;

namespace Web.Pages.Pedido
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public PedidoRequest pedido { get; set; } = new PedidoRequest();

        [BindProperty]
        public int IdCategoria { get; set; }

        [BindProperty]
        public int IdProducto { get; set; }

        [BindProperty]
        public int Cantidad { get; set; } = 0;

        [BindProperty]
        public Guid? IdPedido { get; set; }

        [BindProperty]
        public string NombreUsuario { get; set; } = string.Empty;

        public List<SelectListItem> categorias { get; set; } = new List<SelectListItem>();

        public List<ProductoResponse> productos { get; set; } = new List<ProductoResponse>();

        public Dictionary<int, decimal> stockProductos { get; set; }  = new Dictionary<int, decimal>();

        public List<DetallePedidoResponse> detalles { get; set; } = new List<DetallePedidoResponse>();

        public PedidoResponse? pedidoActual { get; set; }

        public async Task<ActionResult> OnPost()
        {
            await ObtenerCategorias();
            await ObtenerProductos();
            await ObtenerStockProductos();

            var idUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine($"CLAIM USUARIO: {idUsuario}");

            if (string.IsNullOrEmpty(idUsuario))
            {
                return Unauthorized();
            }

            pedido.IdUsuario = Guid.Parse(idUsuario);

            Console.WriteLine($"PEDIDO ID USUARIO: {pedido.IdUsuario}");

            if (IdProducto <= 0)
            {
                ModelState.AddModelError(string.Empty,"Debe seleccionar un producto.");

                return Page();
            }

            if (Cantidad <= 0)
            {
                ModelState.AddModelError(string.Empty,"La cantidad debe ser mayor a cero.");

                return Page();
            }

            string endpointStock = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerStockProducto");

            endpointStock = string.Format( endpointStock, IdProducto);

            var clienteStock = new HttpClient();

            var respuestaStock = await clienteStock.GetAsync(endpointStock);

            if (!respuestaStock.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "No se pudo consultar el stock del producto.");

                return Page();
            }

            var stock = await respuestaStock.Content.ReadFromJsonAsync<StockProductoResponse>();

            if (stock == null ||
                stock.StockDisponible < Cantidad)
            {
                ModelState.AddModelError(string.Empty,$"Stock insuficiente. Disponible: {stock?.StockDisponible ?? 0}.");
             
                if (IdPedido.HasValue)
                {
                    await ObtenerPedido();
                    await ObtenerDetalles();
                }

                return Page();
            }

            var cliente = new HttpClient();

            Guid idPedido;

            if (IdPedido == null || IdPedido == Guid.Empty)
            {
                string endpointPedido = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarPedido");

                var respuestaPedido = await cliente.PostAsJsonAsync(endpointPedido, pedido);

                if (!respuestaPedido.IsSuccessStatusCode)
                {
                    var error = await respuestaPedido.Content.ReadAsStringAsync();

                    ModelState.AddModelError(string.Empty, error);

                    return Page();
                }

                idPedido = await respuestaPedido.Content.ReadFromJsonAsync<Guid>();
            }
            else
            {
                idPedido = IdPedido.Value;
            }

            var detalle = new DetallePedidoRequest
            {
                IdPedido = idPedido,
                IdProducto = IdProducto,
                Cantidad = Cantidad
            };

            string endpointDetalle = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarDetallePedido");

            var respuestaDetalle = await cliente.PostAsJsonAsync(endpointDetalle, detalle);

            if (!respuestaDetalle.IsSuccessStatusCode)
            {
                var error = await respuestaDetalle.Content.ReadAsStringAsync();

                ModelState.AddModelError(string.Empty, error);

                if (IdPedido.HasValue)
                {
                    await ObtenerPedido();
                    await ObtenerDetalles();
                }

                return Page();

            }

            return RedirectToPage("./Agregar", new
                {
                    idPedido = idPedido
                });
        }

        public async Task ObtenerCategorias()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints","ObtenerCategorias");

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get,endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

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

            var resultadoSerializado =
                JsonSerializer.Deserialize<List<Categoria>>(resultado,opciones) ?? new List<Categoria>();

            categorias = resultadoSerializado
                .Select(categoria => new SelectListItem
                {
                    Value = categoria.IdCategoria.ToString(),
                    Text = categoria.Nombre
                })
                .ToList();
        }

        public async Task ObtenerProductos()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints","ObtenerProductos");

            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get,endpoint);

            var respuesta = await cliente.SendAsync(solicitud);

            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                productos = new List<ProductoResponse>();
                return;
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            productos =
                JsonSerializer.Deserialize<List<ProductoResponse>>(resultado, opciones) ?? new List<ProductoResponse>();
        }

        public async Task ObtenerPedido()
        {
            if (!IdPedido.HasValue)
                return;

            string endpoint =_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPedido");

            endpoint = string.Format(endpoint, IdPedido.Value);

            var cliente = new HttpClient();

            var respuesta = await cliente.GetAsync(endpoint);

            respuesta.EnsureSuccessStatusCode();

            pedidoActual = await respuesta.Content.ReadFromJsonAsync<PedidoResponse>();

            if (pedidoActual != null)
            {
                NombreUsuario = pedidoActual.NombreUsuario;
            }
        }

        public async Task ObtenerDetalles()
        {
            if (!IdPedido.HasValue)
                return;

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerDetallesPorPedido");

            endpoint = string.Format( endpoint, IdPedido.Value);

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

        public async Task ObtenerStockProductos()
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

        public async Task OnGet(Guid? idPedido)
        {
            IdPedido = idPedido;

            if (!IdPedido.HasValue)
            {
                var idUsuario = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(idUsuario))
                {
                    pedido.IdUsuario = Guid.Parse(idUsuario);
                }
                NombreUsuario = User.Identity?.Name ?? string.Empty;
            }

            await ObtenerCategorias();
            await ObtenerProductos();
            await ObtenerStockProductos();

            if (idPedido.HasValue && idPedido.Value != Guid.Empty)
            {
                await ObtenerPedido();
                await ObtenerDetalles();
            }
        }
    }
}

