using Abstracciones.Interfaces.Reglas;
using Abstracciones.Web.Modelos.ModeloResenas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Resenas
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        // ==========================================
        // DATOS DE LA RESEÑA
        // ==========================================

        [BindProperty]
        public int IdResena { get; set; }

        [BindProperty]
        public int IdProducto { get; set; }

        [BindProperty]
        public Guid IdUsuario { get; set; }

        [BindProperty]
        public int Calificacion { get; set; }

        [BindProperty]
        public string Comentario { get; set; } = string.Empty;

        public string ErrorResena { get; set; } = string.Empty;


        // ==========================================
        // CONSTRUCTOR
        // ==========================================

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }


        // ==========================================
        // GET - CARGAR RESEÑA
        // ==========================================

        public async Task<IActionResult> OnGetAsync(
            int idResena,
            int idProducto)
        {
            IdResena = idResena;
            IdProducto = idProducto;

            Console.WriteLine("====================================");
            Console.WriteLine("EDITAR RESEÑA - GET");
            Console.WriteLine($"ID RESEÑA: {idResena}");
            Console.WriteLine($"ID PRODUCTO: {idProducto}");
            Console.WriteLine("====================================");

            using var cliente = new HttpClient();

            var endpoint = string.Format(
                _configuracion.ObtenerMetodo(
                    "ApiEndPoints",
                    "ObtenerResena"),
                idResena);

            Console.WriteLine("ENDPOINT GET RESEÑA: " + endpoint);

            var respuesta = await cliente.GetAsync(endpoint);

            var respuestaTexto =
                await respuesta.Content.ReadAsStringAsync();

            Console.WriteLine("STATUS GET: " + respuesta.StatusCode);
            Console.WriteLine("RESPUESTA GET: " + respuestaTexto);

            // ==========================================
            // RESEÑA NO EXISTE
            // ==========================================

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                ErrorResena = "La reseña no existe.";

                return Page();
            }

            // ==========================================
            // ERROR DEL API
            // ==========================================

            if (!respuesta.IsSuccessStatusCode)
            {
                ErrorResena =
                    $"No se pudo obtener la reseña. Error: {respuestaTexto}";

                return Page();
            }

            // ==========================================
            // DESERIALIZAR
            // ==========================================

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var resena =
                JsonSerializer.Deserialize<ResenaResponse>(
                    respuestaTexto,
                    opciones);

            if (resena == null)
            {
                ErrorResena =
                    "No se pudo cargar la información de la reseña.";

                return Page();
            }

            // ==========================================
            // CARGAR DATOS
            // ==========================================

            IdResena = resena.IdResena;

            IdUsuario = resena.IdUsuario;

            IdProducto = resena.IdProducto;

            Calificacion = resena.Calificacion;

            Comentario = resena.Comentario;

            Console.WriteLine("====================================");
            Console.WriteLine("RESEÑA CARGADA");
            Console.WriteLine($"ID RESEÑA: {IdResena}");
            Console.WriteLine($"ID USUARIO: {IdUsuario}");
            Console.WriteLine($"ID PRODUCTO: {IdProducto}");
            Console.WriteLine($"CALIFICACION: {Calificacion}");
            Console.WriteLine($"COMENTARIO: {Comentario}");
            Console.WriteLine("====================================");

            return Page();
        }


        // ==========================================
        // POST - EDITAR RESEÑA
        // ==========================================

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("====================================");
            Console.WriteLine("EDITAR RESEÑA - POST");
            Console.WriteLine($"ID RESEÑA: {IdResena}");
            Console.WriteLine($"ID USUARIO: {IdUsuario}");
            Console.WriteLine($"ID PRODUCTO: {IdProducto}");
            Console.WriteLine($"CALIFICACION: {Calificacion}");
            Console.WriteLine($"COMENTARIO: {Comentario}");
            Console.WriteLine("====================================");

            // ==========================================
            // VALIDAR ID RESEÑA
            // ==========================================

            if (IdResena <= 0)
            {
                ErrorResena = "El ID de la reseña no es válido.";

                return Page();
            }

            // ==========================================
            // VALIDAR ID PRODUCTO
            // ==========================================

            if (IdProducto <= 0)
            {
                ErrorResena = "El producto no es válido.";

                return Page();
            }

            // ==========================================
            // VALIDAR USUARIO
            // ==========================================

            if (IdUsuario == Guid.Empty)
            {
                ErrorResena =
                    "No se pudo identificar al usuario de la reseña.";

                return Page();
            }

            // ==========================================
            // VALIDAR CALIFICACIÓN
            // ==========================================

            if (Calificacion < 1 || Calificacion > 5)
            {
                ErrorResena =
                    "La calificación debe estar entre 1 y 5.";

                return Page();
            }

            // ==========================================
            // VALIDAR COMENTARIO
            // ==========================================

            if (string.IsNullOrWhiteSpace(Comentario))
            {
                ErrorResena =
                    "Debe escribir un comentario.";

                return Page();
            }

            // ==========================================
            // CREAR REQUEST
            // ==========================================

            var resena = new ResenaRequest
            {
                IdUsuario = IdUsuario,
                IdProducto = IdProducto,
                Calificacion = Calificacion,
                Comentario = Comentario
            };

            // ==========================================
            // CLIENTE HTTP
            // ==========================================

            using var cliente = new HttpClient();

            // ==========================================
            // OBTENER TOKEN SI EXISTE
            // ==========================================

            var token = User.FindFirst("Token")?.Value;

            if (!string.IsNullOrWhiteSpace(token))
            {
                cliente.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(
                        "Bearer",
                        token);
            }

            // ==========================================
            // ENDPOINT
            // ==========================================

            var endpoint = string.Format(
                _configuracion.ObtenerMetodo(
                    "ApiEndPoints",
                    "EditarResena"),
                IdResena);

            Console.WriteLine("ENDPOINT PUT: " + endpoint);

            // ==========================================
            // SERIALIZAR
            // ==========================================

            var json =
                JsonSerializer.Serialize(resena);

            Console.WriteLine("JSON ENVIADO:");
            Console.WriteLine(json);

            var contenido =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

            // ==========================================
            // PUT
            // ==========================================

            var respuesta =
                await cliente.PutAsync(
                    endpoint,
                    contenido);

            var respuestaTexto =
                await respuesta.Content.ReadAsStringAsync();

            Console.WriteLine("====================================");
            Console.WriteLine("RESPUESTA PUT");
            Console.WriteLine($"STATUS: {respuesta.StatusCode}");
            Console.WriteLine($"RESPUESTA: {respuestaTexto}");
            Console.WriteLine("====================================");

            // ==========================================
            // RESEÑA NO EXISTE
            // ==========================================

            if (respuesta.StatusCode == HttpStatusCode.NotFound)
            {
                ErrorResena =
                    "La reseña no existe.";

                return Page();
            }

            // ==========================================
            // NO AUTORIZADO
            // ==========================================

            if (respuesta.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                ErrorResena =
                    "Tu sesión no es válida. Inicia sesión nuevamente.";

                return Page();
            }

            // ==========================================
            // OTRO ERROR
            // ==========================================

            if (!respuesta.IsSuccessStatusCode)
            {
                ErrorResena =
                    $"No se pudo editar la reseña. Error: {respuestaTexto}";

                return Page();
            }

            // ==========================================
            // EDICIÓN EXITOSA
            // ==========================================

            Console.WriteLine("RESEÑA EDITADA CORRECTAMENTE.");

            return RedirectToPage(
                "/Productos/Detalle",
                new
                {
                    id = IdProducto
                });
        }
    }
}