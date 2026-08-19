using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Promociones
{
    public abstract class PromocionFormPage : PageModel
    {
        protected readonly IConfiguracion Configuracion;
        protected PromocionFormPage(IConfiguracion configuracion) => Configuracion = configuracion;

        [BindProperty] public PromocionRequest Promocion { get; set; } = new();
        public List<SelectListItem> Productos { get; set; } = new();
        public List<SelectListItem> Categorias { get; set; } = new();

        protected async Task CargarOpciones()
        {
            await CargarProductos();
            await CargarCategorias();
        }

        private async Task CargarProductos()
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(Configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProductos"));
            if (respuesta.StatusCode == HttpStatusCode.NoContent) return;
            respuesta.EnsureSuccessStatusCode();
            var datos = JsonSerializer.Deserialize<List<ProductoResponse>>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            Productos = datos.Select(p => new SelectListItem(p.Nombre, p.IdProducto.ToString(), Promocion.IdsProductos.Contains(p.IdProducto))).ToList();
        }

        private async Task CargarCategorias()
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(Configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias"));
            if (respuesta.StatusCode == HttpStatusCode.NoContent) return;
            respuesta.EnsureSuccessStatusCode();
            var datos = JsonSerializer.Deserialize<List<CategoriaResponse>>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            Categorias = datos.Select(c => new SelectListItem(c.Nombre, c.IdCategoria.ToString(), Promocion.IdsCategorias.Contains(c.IdCategoria))).ToList();
        }

        protected bool ValidarNegocio()
        {
            if (Promocion.FechaFin < Promocion.FechaInicio)
                ModelState.AddModelError("Promocion.FechaFin", "La fecha final no puede ser anterior a la inicial.");
            if (Promocion.TipoDescuento == "PORCENTAJE" && Promocion.ValorDescuento > 100)
                ModelState.AddModelError("Promocion.ValorDescuento", "El porcentaje no puede superar 100.");
            if (!Promocion.IdsProductos.Any() && !Promocion.IdsCategorias.Any())
                ModelState.AddModelError("Promocion.IdsProductos", "Seleccione al menos un producto o una categoría.");
            return ModelState.IsValid;
        }
    }
}
