using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;

namespace Web.Pages.Categorias
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        public CategoriaResponse Categoria { get; private set; } = new();

        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategoria"), id);
            using var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(endpoint);
            if (respuesta.StatusCode == HttpStatusCode.NotFound) return NotFound();
            respuesta.EnsureSuccessStatusCode();
            Categoria = await respuesta.Content.ReadFromJsonAsync<CategoriaResponse>() ?? new CategoriaResponse();
            return Page();
        }
    }
}
