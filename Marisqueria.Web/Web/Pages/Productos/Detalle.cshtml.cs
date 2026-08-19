using Abstracciones.Interfaces.Reglas;
using Abstracciones.Modelo;
using Abstracciones.Web.Modelos.ModeloResenas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class DetalleModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public ProductoResponse producto { get; set; } = new ProductoResponse();

        public PrecioPromocionalResponse? Promocion { get; set; }


        public List<ResenaResponse> Resenas { get; set; } = new();

       
        [BindProperty]
        public int Calificacion { get; set; }

        [BindProperty]
        public string Comentario { get; set; } = string.Empty;

        public string ErrorResena { get; set; } = string.Empty;


      
        public DetalleModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }



        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            return await CargarPagina(id.Value);
        }


      
        public async Task<IActionResult> OnPostAsync(int id)
        {
      

            if (Calificacion < 1 || Calificacion > 5)
            {
                ErrorResena =
                    "Debe seleccionar una calificación.";

                return await CargarPagina(id);
            }


            if (string.IsNullOrWhiteSpace(Comentario))
            {
                ErrorResena =
                    "Debe escribir un comentario.";

                return await CargarPagina(id);
            }


           
            var token =
                User.FindFirst("Token")?.Value;


            if (string.IsNullOrWhiteSpace(token))
            {
                ErrorResena =
                    "Debe iniciar sesión para escribir una reseña.";

                return await CargarPagina(id);
            }



            var idUsuarioClaim =
                User.FindFirst(
                    ClaimTypes.NameIdentifier
                )?.Value;


            if (!Guid.TryParse(
                    idUsuarioClaim,
                    out Guid idUsuario))
            {
                ErrorResena =
                    "No se pudo identificar al usuario.";

                return await CargarPagina(id);
            }


        
            var resena = new ResenaRequest
            {
                IdProducto = id,
                IdUsuario = idUsuario,
                Calificacion = Calificacion,
                Comentario = Comentario
            };


           

            using var cliente = new HttpClient();


         
            cliente.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);



            var endpointResenas =
                _configuracion.ObtenerMetodo(
                    "ApiEndPoints",
                    "AgregarResena");


        


            var json =
                JsonSerializer.Serialize(resena);


         


            var contenido =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");


            var respuesta =
                await cliente.PostAsync(
                    endpointResenas,
                    contenido);


        

            var respuestaTexto =
                await respuesta.Content
                    .ReadAsStringAsync();


           
            if (respuesta.StatusCode ==
                HttpStatusCode.Unauthorized)
            {
                ErrorResena =
                    "Tu sesión no es válida. Inicia sesión nuevamente.";

                return await CargarPagina(id);
            }


          

            if (!respuesta.IsSuccessStatusCode)
            {
                ErrorResena =
                    "No se pudo guardar la reseña. " +
                    "Error " +
                    respuesta.StatusCode +
                    ": " +
                    respuestaTexto;

                return await CargarPagina(id);
            }


         


            return RedirectToPage(
                "/Productos/Detalle",
                new
                {
                    id = id
                });
        }


        public async Task<IActionResult> OnPostEditarResenaAsync(
            int id,
            int idResena,
            int calificacion,
            string comentario)
        {
            // ==========================================
            // VALIDAR ID RESEÑA
            // ==========================================

            if (idResena <= 0)
            {
                ErrorResena =
                    "La reseña no es válida.";

                return await CargarPagina(id);
            }


          

            Guid idUsuario = Guid.Parse(
                "8DAF7F37-4535-413C-8B98-17035EDA7270"
            );


           
            var resena = new ResenaRequest
            {
                IdProducto = id,
                IdUsuario = idUsuario,
                Calificacion = calificacion,
                Comentario = comentario
            };


            

            if (calificacion < 1 || calificacion > 5)
            {
                ErrorResena =
                    "La calificación debe estar entre 1 y 5.";

                return await CargarPagina(id);
            }


            if (string.IsNullOrWhiteSpace(comentario))
            {
                ErrorResena =
                    "Debe escribir un comentario.";

                return await CargarPagina(id);
            }


         

            using var cliente = new HttpClient();



            var endpoint =
                string.Format(
                    _configuracion.ObtenerMetodo(
                        "ApiEndPoints",
                        "EditarResena"),
                    idResena
                );




          
            var json =
                JsonSerializer.Serialize(resena);


            Console.WriteLine("JSON:");
            Console.WriteLine(json);


            var contenido =
                new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");


         

            var respuesta =
                await cliente.PutAsync(
                    endpoint,
                    contenido);


           

            var respuestaTexto =
                await respuesta.Content
                    .ReadAsStringAsync();


            Console.WriteLine("STATUS: " +
                respuesta.StatusCode);

            Console.WriteLine("RESPUESTA: " +
                respuestaTexto);


          

            if (!respuesta.IsSuccessStatusCode)
            {
                ErrorResena =
                    $"No se pudo editar la reseña. " +
                    $"Error {respuesta.StatusCode}: " +
                    respuestaTexto;

                return await CargarPagina(id);
            }



            return RedirectToPage(
                "/Productos/Detalle",
                new
                {
                    id = id
                });
        }


       

        public async Task<IActionResult> OnPostEliminarResenaAsync(
            int id,
            int idResena)
        {
           

            if (idResena <= 0)
            {
                ErrorResena =
                    "La reseña no es válida.";

                return await CargarPagina(id);
            }


           

            using var cliente = new HttpClient();


            
            var endpoint =
                string.Format(
                    _configuracion.ObtenerMetodo(
                        "ApiEndPoints",
                        "EliminarResena"),
                    idResena
                );




         

            var respuesta =
                await cliente.DeleteAsync(
                    endpoint);


         

            var respuestaTexto =
                await respuesta.Content
                    .ReadAsStringAsync();


        


         

            if (!respuesta.IsSuccessStatusCode)
            {
                ErrorResena =
                    $"No se pudo eliminar la reseña. " +
                    $"Error {respuesta.StatusCode}: " +
                    respuestaTexto;

                return await CargarPagina(id);
            }


            return RedirectToPage(
                "/Productos/Detalle",
                new
                {
                    id = id
                });
        }


      
        private async Task<IActionResult> CargarPagina(int id)
        {
            using var cliente =
                new HttpClient();


            var opciones =
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };


        

            var endpointProducto =
                string.Format(
                    _configuracion.ObtenerMetodo(
                        "ApiEndPoints",
                        "ObtenerProducto"),
                    id);


            var respuestaProducto =
                await cliente.GetAsync(
                    endpointProducto);


            if (respuestaProducto.StatusCode ==
                HttpStatusCode.NotFound)
            {
                return NotFound();
            }


            respuestaProducto.EnsureSuccessStatusCode();


            var contenidoProducto =
                await respuestaProducto.Content
                    .ReadAsStringAsync();


            producto =
                JsonSerializer.Deserialize<ProductoResponse>(
                    contenidoProducto,
                    opciones
                )
                ?? new ProductoResponse();


 

            var endpointResenas =
                string.Format(
                    _configuracion.ObtenerMetodo(
                        "ApiEndPoints",
                        "ObtenerResenasPorProducto"),
                    id);


            var respuestaResenas =
                await cliente.GetAsync(
                    endpointResenas);


            if (respuestaResenas.IsSuccessStatusCode &&
                respuestaResenas.StatusCode !=
                HttpStatusCode.NoContent)
            {
                var contenidoResenas =
                    await respuestaResenas.Content
                        .ReadAsStringAsync();


                Resenas =
                    JsonSerializer.Deserialize<
                        List<ResenaResponse>>(
                            contenidoResenas,
                            opciones
                        )
                    ?? new List<ResenaResponse>();
            }
            else
            {
                Resenas = new List<ResenaResponse>();
            }



            var endpointPromo =
                string.Format(
                    _configuracion.ObtenerMetodo(
                        "ApiEndPoints",
                        "ObtenerPrecioPromocionalProducto"),
                    id);


            var promoResponse =
                await cliente.GetAsync(
                    endpointPromo);


            if (promoResponse.IsSuccessStatusCode &&
                promoResponse.StatusCode !=
                HttpStatusCode.NoContent)
            {
                var contenidoPromo =
                    await promoResponse.Content
                        .ReadAsStringAsync();


                Promocion =
                    JsonSerializer.Deserialize<
                        PrecioPromocionalResponse>(
                            contenidoPromo,
                            opciones);
            }
            else
            {
                Promocion = null;
            }


            return Page();
        }
    }
}
