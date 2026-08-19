using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloAutenticacion;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Net.Http.Json;

namespace Web.Pages.Autenticacion
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public LoginModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public LoginRequest Login { get; set; } = new();

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
                "LoginUsuario");

            using var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(
                url,
                Login);

            if (!respuesta.IsSuccessStatusCode)
            {
                Error = "Correo o contraseña incorrectos.";
                return Page();
            }

            var usuario =
                await respuesta.Content
                    .ReadFromJsonAsync<LoginResponse>();

            if (usuario == null)
            {
                Error = "No se pudo iniciar sesión.";
                return Page();
            }

            Console.WriteLine($"ID LOGIN: {usuario.Id}");
            Console.WriteLine($"NOMBRE LOGIN: {usuario.Nombre}");

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Nombre),

                new Claim(
                    ClaimTypes.Email,
                    usuario.Correo),

                new Claim(
                    ClaimTypes.Role,
                    usuario.Rol),

                new Claim(
                    "Token",
                    usuario.Token)
            };

            var identidad = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(
                identidad);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);

            return RedirectToPage("/Index");
        }
    }
}
