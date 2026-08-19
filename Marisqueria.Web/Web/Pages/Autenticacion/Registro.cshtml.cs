using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloAutenticacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace Web.Pages.Autenticacion
{
    public class RegistroModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public RegistroModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public RegistroRequest Registro { get; set; } = new();

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
                "AgregarUsuario");

            using var cliente = new HttpClient();

            var usuarioParaApi = new
            {
                nombre = Registro.Nombre,
                correo = Registro.Correo,
                telefono = Registro.Telefono,
                contrasena = Registro.Contrasena
            };

            try
            {
                var respuesta = await cliente.PostAsJsonAsync(
                    url,
                    usuarioParaApi);

                if (!respuesta.IsSuccessStatusCode)
                {
                    if (respuesta.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        Error = "Verifique los datos ingresados. La contraseña debe cumplir con los requisitos.";
                    }
                    else
                    {
                        Error = "No se pudo registrar el usuario. Inténtelo nuevamente.";
                    }

                    return Page();
                }

                TempData["Mensaje"] = "Usuario registrado correctamente.";

                return RedirectToPage("/Autenticacion/Login");
            }
            catch
            {
                Error = "No fue posible conectarse con el servidor.";

                return Page();
            }
        }
    }
}