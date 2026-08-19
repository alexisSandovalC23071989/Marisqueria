using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Promociones
{
    public class AlertasModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<AlertaPromocionResponse> Alertas { get; set; } = new List<AlertaPromocionResponse>();
        [BindProperty(SupportsGet = true)] public int DiasAnticipacion { get; set; } = 3;
        [BindProperty(SupportsGet = true)] public bool MostrarRevisadas { get; set; }

        public AlertasModel(IConfiguracion configuracion) => _configuracion = configuracion;

        public async Task OnGet()
        {
            if (DiasAnticipacion < 1 || DiasAnticipacion > 30) DiasAnticipacion = 3;
            await Cargar();
        }

        public async Task<IActionResult> OnPostRevisar(int idAlerta)
        {
            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "RevisarAlertaPromocion"), idAlerta);
            var respuesta = await cliente.PatchAsync(endpoint, null);
            if (respuesta.StatusCode != HttpStatusCode.NoContent && !respuesta.IsSuccessStatusCode)
                TempData["Error"] = "No fue posible marcar la alerta como revisada.";
            return RedirectToPage("./Alertas", new { DiasAnticipacion, MostrarRevisadas });
        }

        private async Task Cargar()
        {
            using var cliente = new HttpClient();
            var endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerAlertasPromociones")
                + $"?diasAnticipacion={DiasAnticipacion}&soloPendientes={!MostrarRevisadas}";
            var respuesta = await cliente.GetAsync(endpoint);
            if (respuesta.StatusCode == HttpStatusCode.NoContent) return;
            respuesta.EnsureSuccessStatusCode();
            Alertas = JsonSerializer.Deserialize<List<AlertaPromocionResponse>>(
                await respuesta.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }
}
