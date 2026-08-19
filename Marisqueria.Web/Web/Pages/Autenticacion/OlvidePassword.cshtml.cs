using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloAutenticacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace Web.Pages.Autenticacion
{
    public class OlvidePasswordModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public OlvidePasswordModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public SolicitarRecuperacionRequest Solicitud { get; set; } = new();

        public string Mensaje { get; set; } = string.Empty;

        public string Error { get; set; } = string.Empty;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var url = _configuracion.ObtenerMetodo(
                "ApiEndPoints",
                "SolicitarRecuperacion");

            using var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(
                url,
                Solicitud);

            if (!respuesta.IsSuccessStatusCode)
            {
                Error = await respuesta.Content.ReadAsStringAsync();
                return Page();
            }

            Mensaje =
                "Se envió un enlace de recuperación a tu correo.";

            return Page();
        }
    }
}