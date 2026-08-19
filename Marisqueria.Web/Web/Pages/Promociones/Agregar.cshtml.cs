using Abstracciones.Interfaces.Reglas;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace Web.Pages.Promociones
{
    public class AgregarModel : PromocionFormPage
    {
        public AgregarModel(IConfiguracion configuracion) : base(configuracion) { }
        public async Task OnGet() => await CargarOpciones();
        public async Task<IActionResult> OnPost()
        {
            if (!ValidarNegocio()) { await CargarOpciones(); return Page(); }
            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var respuesta = await cliente.PostAsJsonAsync(Configuracion.ObtenerMetodo("ApiEndPoints", "AgregarPromocion"), Promocion);
            if (!respuesta.IsSuccessStatusCode) { ModelState.AddModelError(string.Empty, await respuesta.Content.ReadAsStringAsync()); await CargarOpciones(); return Page(); }
            return RedirectToPage("./Index");
        }
    }
}
