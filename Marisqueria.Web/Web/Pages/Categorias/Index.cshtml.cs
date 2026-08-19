using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text.Json;

namespace Web.Pages.Categorias
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public IList<CategoriaResponse> Categorias { get; private set; } = new List<CategoriaResponse>();

        public IndexModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task OnGetAsync()
        {
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias"));
            respuesta.EnsureSuccessStatusCode();
            if (respuesta.StatusCode == HttpStatusCode.NoContent) return;

            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Categorias = JsonSerializer.Deserialize<List<CategoriaResponse>>(await respuesta.Content.ReadAsStringAsync(), opciones)
                ?? new List<CategoriaResponse>();
        }

        public async Task<IActionResult> OnPostEliminarAsync(int id)
        {
            if (!User.IsInRole("Administrador")) return Forbid();

            string endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "EliminarCategoria"), id);
            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var respuesta = await cliente.DeleteAsync(endpoint);

            if (respuesta.StatusCode == HttpStatusCode.Conflict)
            {
                TempData["Error"] = "La categoría no se puede eliminar porque tiene productos asociados.";
                return RedirectToPage();
            }

            respuesta.EnsureSuccessStatusCode();
            TempData["Mensaje"] = "Categoría eliminada correctamente.";
            return RedirectToPage();
        }
    }
}
