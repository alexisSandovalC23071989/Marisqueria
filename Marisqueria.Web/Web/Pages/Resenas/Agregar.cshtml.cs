using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloResenas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Resenas
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        [BindProperty]
        public ResenaRequest Resena { get; set; } = new();

        public string Error { get; set; } = string.Empty;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        // ==========================================
        // GET
        // ==========================================

        public IActionResult OnGet(int idProducto)
        {
            if (idProducto <= 0)
            {
                return NotFound();
            }

            Resena.IdProducto = idProducto;

            return Page();
        }

        // ==========================================
        // POST
        // ==========================================

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("========== POST AGREGAR RESEÑA ==========");

            Console.WriteLine(
                "ID PRODUCTO: " + Resena.IdProducto
            );

            // ==========================================
            // VALIDAR CALIFICACIÓN
            // ==========================================

            if (Resena.Calificacion < 1 ||
                Resena.Calificacion > 5)
            {
                Error = "Debe seleccionar una calificación.";
                return Page();
            }

            // ==========================================
            // VALIDAR COMENTARIO
            // ==========================================

            if (string.IsNullOrWhiteSpace(Resena.Comentario))
            {
                Error = "Debe escribir un comentario.";
                return Page();
            }

            // ==========================================
            // OBTENER USUARIO
            // ==========================================

            var idUsuarioClaim =
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Console.WriteLine(
                "ID USUARIO DEL LOGIN: " + idUsuarioClaim
            );

            if (!Guid.TryParse(
                    idUsuarioClaim,
                    out Guid idUsuario))
            {
                Error =
                    "No se pudo identificar al usuario. Inicie sesión nuevamente.";

                return Page();
            }

            // ==========================================
            // ASIGNAR USUARIO
            // ==========================================

            Resena.IdUsuario = idUsuario;

            // ==========================================
            // MOSTRAR JSON
            // ==========================================

            var json =
                JsonSerializer.Serialize(Resena);

            Console.WriteLine("=================================");
            Console.WriteLine(
                "ID USUARIO ENVIADO: " +
                Resena.IdUsuario
            );

            Console.WriteLine(
                "ID PRODUCTO ENVIADO: " +
                Resena.IdProducto
            );

            Console.WriteLine(
                "JSON ENVIADO: " +
                json
            );

            Console.WriteLine("=================================");

            // ==========================================
            // CLIENTE HTTP
            // ==========================================

            using var cliente = new HttpClient();

            // ==========================================
            // ENDPOINT
            // ==========================================

            var endpoint =
                _configuracion.ObtenerMetodo(
                    "ApiEndPoints",
                    "AgregarResena"
                );

            Console.WriteLine(
                "ENDPOINT POST: " + endpoint
            );

            // ==========================================
            // CONTENIDO
            // ==========================================

            var contenido =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

            // ==========================================
            // POST API
            // ==========================================

            var respuesta =
                await cliente.PostAsync(
                    endpoint,
                    contenido
                );

            // ==========================================
            // RESPUESTA
            // ==========================================

            var respuestaTexto =
                await respuesta.Content.ReadAsStringAsync();

            Console.WriteLine(
                "STATUS POST: " +
                respuesta.StatusCode
            );

            Console.WriteLine(
                "RESPUESTA API: " +
                respuestaTexto
            );

            Console.WriteLine("=================================");

            // ==========================================
            // ERROR
            // ==========================================

            if (!respuesta.IsSuccessStatusCode)
            {
                Error =
                    $"No se pudo agregar la reseña. " +
                    $"Error {respuesta.StatusCode}: " +
                    respuestaTexto;

                return Page();
            }

            // ==========================================
            // ÉXITO
            // ==========================================

            return RedirectToPage(
                "/Productos/Detalle",
                new
                {
                    id = Resena.IdProducto
                }
            );
        }
    }
}