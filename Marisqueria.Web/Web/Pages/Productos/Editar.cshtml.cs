using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoResponse productoResponse { get; set; } = new ProductoResponse();

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new List<SelectListItem>();

        public async Task<ActionResult> OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            await ObtenerCategorias();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                return NotFound();
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            productoResponse = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones) ?? new ProductoResponse();

            foreach (var categoria in categorias)
            {
                categoria.Selected = categoria.Value == productoResponse.IdCategoria.ToString();
            }

            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                await ObtenerCategorias();
                return Page();
            }

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);

            var respuesta = await cliente.PutAsJsonAsync(string.Format(endpoint, productoResponse.IdProducto), new ProductoRequest
            {
                IdCategoria = productoResponse.IdCategoria,
                Nombre = productoResponse.Nombre,
                Descripcion = productoResponse.Descripcion,
                PrecioVentaActual = productoResponse.PrecioVentaActual,
                Imagen = productoResponse.Imagen
            });
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        public async Task ObtenerCategorias()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                categorias = new List<SelectListItem>();
                return;
            }

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var resultadoSerializado = JsonSerializer.Deserialize<List<Categoria>>(resultado, opciones) ?? new List<Categoria>();

            categorias = resultadoSerializado.Select(categoria =>
                new SelectListItem
                {
                    Value = categoria.IdCategoria.ToString(),
                    Text = categoria.Nombre
                }
            ).ToList();
        }
    }
}

