using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Json;

namespace Web.Pages.Categorias
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        [BindProperty] public CategoriaResponse Categoria { get; set; } = new();

        public EditarModel(IConfiguracion configuracion)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            string endpoint = string.Format(_configuracion.ObtenerMetodo("ApiEndPoints", "EditarCategoria"), Categoria.IdCategoria);
            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var respuesta = await cliente.PutAsJsonAsync(endpoint, new CategoriaRequest { Nombre = Categoria.Nombre });
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, await respuesta.Content.ReadAsStringAsync());
                return Page();
            }
            return RedirectToPage("./Index");
        }
    }
}
