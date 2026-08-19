using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json;

namespace Web.Pages.Promociones
{
    public class EditarModel : PromocionFormPage
    {
        [BindProperty(SupportsGet = true)] public int Id { get; set; }
        public EditarModel(IConfiguracion configuracion) : base(configuracion) { }
        public async Task<IActionResult> OnGet()
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(string.Format(Configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPromocion"), Id));
            if (!respuesta.IsSuccessStatusCode) return RedirectToPage("./Index");
            var actual = JsonSerializer.Deserialize<PromocionResponse>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (actual == null) return RedirectToPage("./Index");
            Promocion = actual;
            Promocion.IdsProductos = actual.Productos.Where(x => x.AsignacionDirecta).Select(x => x.IdProducto).ToList();
            Promocion.IdsCategorias = actual.Categorias.Select(x => x.IdCategoria).ToList();
            await CargarOpciones();
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ValidarNegocio()) { await CargarOpciones(); return Page(); }
            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var endpoint = string.Format(Configuracion.ObtenerMetodo("ApiEndPoints", "EditarPromocion"), Id);
            var respuesta = await cliente.PutAsJsonAsync(endpoint, Promocion);
            if (!respuesta.IsSuccessStatusCode) { ModelState.AddModelError(string.Empty, await respuesta.Content.ReadAsStringAsync()); await CargarOpciones(); return Page(); }
            return RedirectToPage("./Index");
        }
    }
}
