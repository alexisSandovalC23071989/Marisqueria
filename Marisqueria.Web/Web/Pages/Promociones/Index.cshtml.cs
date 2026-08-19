using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Promociones
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<PromocionResponse> Promociones { get; set; } = new List<PromocionResponse>();
        public string? Estado { get; set; }

        public IndexModel(IConfiguracion configuracion) => _configuracion = configuracion;

        public async Task OnGet(string? estado)
        {
            Estado = estado;
            var endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerPromociones");
            if (!string.IsNullOrWhiteSpace(estado)) endpoint += $"?estado={Uri.EscapeDataString(estado)}";
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(endpoint);
            if (respuesta.StatusCode == HttpStatusCode.NoContent) return;
            respuesta.EnsureSuccessStatusCode();
            Promociones = JsonSerializer.Deserialize<List<PromocionResponse>>(
                await respuesta.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
    }
}
