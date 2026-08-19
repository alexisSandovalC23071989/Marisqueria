using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<ProductoResponse> productos { get; set; } = new List<ProductoResponse>();
        public Dictionary<int, PrecioPromocionalResponse> PromocionesPorProducto { get; set; } = new();

        public IndexModel(IConfiguracion configuracion) => _configuracion = configuracion;

        public async Task OnGet()
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos"));
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                productos = new List<ProductoResponse>();
                return;
            }

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            productos = JsonSerializer.Deserialize<List<ProductoResponse>>(await respuesta.Content.ReadAsStringAsync(), opciones) ?? new();

            foreach (var producto in productos)
            {
                var endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPrecioPromocionalProducto"), producto.IdProducto);
                var promoResponse = await cliente.GetAsync(endpoint);
                if (promoResponse.StatusCode == HttpStatusCode.NoContent) continue;
                if (!promoResponse.IsSuccessStatusCode) continue;

                var promo = JsonSerializer.Deserialize<PrecioPromocionalResponse>(await promoResponse.Content.ReadAsStringAsync(), opciones);
                if (promo != null) PromocionesPorProducto[producto.IdProducto] = promo;
            }
        }
    }
}
