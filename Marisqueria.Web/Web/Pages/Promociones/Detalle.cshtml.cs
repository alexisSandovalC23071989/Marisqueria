using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Promociones
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public PromocionResponse Promocion { get; set; } = new();
        public DetalleModel(IConfiguracion configuracion) => _configuracion = configuracion;
        public async Task<IActionResult> OnGet(int id)
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPromocion"), id));
            if (!respuesta.IsSuccessStatusCode) return RedirectToPage("./Index");
            Promocion = JsonSerializer.Deserialize<PromocionResponse>(await respuesta.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            return Page();
        }
    }
}
