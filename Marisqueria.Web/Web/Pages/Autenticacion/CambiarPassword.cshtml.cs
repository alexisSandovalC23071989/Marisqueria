using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloAutenticacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace Web.Pages.Autenticacion
{
    public class CambiarPasswordModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public CambiarPasswordModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public CambiarPasswordRequest Solicitud { get; set; } = new();

        public string Error { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public void OnGet(string token)
        {
            Solicitud.Token = token;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var url = _configuracion.ObtenerMetodo(
                "ApiEndPoints",
                "CambiarPassword");

            using var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(
                url,
                Solicitud);

            if (!respuesta.IsSuccessStatusCode)
            {
                Error = await respuesta.Content.ReadAsStringAsync();
                return Page();
            }

            Mensaje = "La contraseña fue actualizada correctamente.";

            return Page();
        }
    }
}