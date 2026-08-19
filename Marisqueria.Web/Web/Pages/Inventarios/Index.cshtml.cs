using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Inventarios
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<InventarioResponse> Inventarios { get; private set; } = new List<InventarioResponse>();

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGetAsync()
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerInventarios"));
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.NoContent) return;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Inventarios = JsonSerializer.Deserialize<List<InventarioResponse>>(await respuesta.Content.ReadAsStringAsync(), opciones)
                ?? new List<InventarioResponse>();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (!User.IsInRole("Administrador")) return Forbid();

            string endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "EliminarInventario"), id);
            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var respuesta = await cliente.DeleteAsync(endpoint);
            respuesta.EnsureSuccessStatusCode();
            TempData["Mensaje"] = "Registro de inventario eliminado correctamente.";
            return RedirectToPage();
        }
    }
}
