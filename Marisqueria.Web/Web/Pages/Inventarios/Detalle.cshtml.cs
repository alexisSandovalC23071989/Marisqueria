using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;

namespace Web.Pages.Inventarios
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public InventarioResponse Inventario { get; private set; } = new();

        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerInventario"), id);
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(endpoint);
            if (respuesta.StatusCode == HttpStatusCode.NotFound) return NotFound();
            respuesta.EnsureSuccessStatusCode();
            Inventario = await respuesta.Content.ReadFromJsonAsync<InventarioResponse>() ?? new InventarioResponse();
            return Page();
        }
    }
}
