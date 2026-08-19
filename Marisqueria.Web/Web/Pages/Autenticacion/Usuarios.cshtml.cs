using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloAutenticacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Web.Pages.Autenticacion
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public UsuariosModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public List<UsuarioResponse> Usuarios { get; set; } = new();

        public string Error { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync()
        {
            var token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Autenticacion/Login");
            }

            var url = _configuracion.ObtenerMetodo(
                "ApiEndPoints",
                "ObtenerUsuarios");

            using var cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var respuesta = await cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.NoContent)
            {
                Usuarios = new List<UsuarioResponse>();
                return Page();
            }

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/Autenticacion/Login");
            }

            if (respuesta.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/Autenticacion/AccesoDenegado");
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                Error = "No se pudieron obtener los usuarios.";
                return Page();
            }

            Usuarios =
                await respuesta.Content
                    .ReadFromJsonAsync<List<UsuarioResponse>>()
                ?? new List<UsuarioResponse>();

            return Page();
        }
    }
}