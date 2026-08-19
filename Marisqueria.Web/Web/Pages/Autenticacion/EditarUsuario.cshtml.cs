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
    public class EditarUsuarioModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarUsuarioModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public UsuarioEditarRequest Usuario { get; set; } = new();

        public string Error { get; set; } = string.Empty;

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Autenticacion/Login");
            }

            var url = _configuracion.ObtenerMetodo(
                "ApiEndPoints",
                "ObtenerUsuario");

            url = string.Format(url, id);

            using var cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var respuesta = await cliente.GetAsync(url);

            if (respuesta.StatusCode == HttpStatusCode.Unauthorized)
            {
                return RedirectToPage("/Autenticacion/Login");
            }

            if (respuesta.StatusCode == HttpStatusCode.Forbidden)
            {
                return RedirectToPage("/Autenticacion/AccesoDenegado");
            }

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound();
            }

            if (!respuesta.IsSuccessStatusCode)
            {
                Error = "No se pudo obtener el usuario.";
                return Page();
            }

            var usuario =
                await respuesta.Content
                    .ReadFromJsonAsync<UsuarioResponse>();

            if (usuario == null)
            {
                Error = "No se encontró el usuario.";
                return Page();
            }

            Usuario = new UsuarioEditarRequest
            {
                Nombre = usuario.Nombre,
                Correo = usuario.Correo,
                Telefono = usuario.Telefono,
                IdRol = usuario.IdRol
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var token = User.FindFirst("Token")?.Value;

            if (string.IsNullOrWhiteSpace(token))
            {
                return RedirectToPage("/Autenticacion/Login");
            }

            var url = _configuracion.ObtenerMetodo(
                "ApiEndPoints",
                "EditarUsuario");

            url = string.Format(url, id);

            using var cliente = new HttpClient();

            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);

            var respuesta = await cliente.PutAsJsonAsync(
                url,
                Usuario);

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
                Error = await respuesta.Content.ReadAsStringAsync();
                return Page();
            }

            return RedirectToPage("/Autenticacion/Usuarios");
        }
    }
}