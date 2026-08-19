using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace Web.Pages.Categorias
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;
        [BindProperty] public CategoriaRequest Categoria { get; set; } = new();

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            using var cliente = new HttpClient();
            cliente.AgregarAutorizacion(User);
            var respuesta = await cliente.PostAsJsonAsync(_configuracion.ObtenerMetodo("ApiEndPoints", "AgregarCategoria"), Categoria);
            if (!respuesta.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, await respuesta.Content.ReadAsStringAsync());
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
